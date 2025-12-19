using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GameApp.Presenter.Commands;
using GameApp.Presenter.Dto;
using GameApp.Presenter.Navigation;
using GameApp.Presenter.Services;
using Logic;

namespace GameApp.Presenter.ViewModels
{
    /// <summary>
    /// Главная ViewModel, связывающая View, бизнес-логику и навигацию.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IGameLogic _logic;
        private readonly IUserInteractionService _userInteractionService;
        private readonly IFileDialogService _fileDialogService;
        private readonly RelayCommand _editCommand;
        private readonly RelayCommand _deleteCommand;

        private GameDto? _selectedGame;
        private string _nameInput = string.Empty;
        private string _genreInput = string.Empty;
        private double _ratingInput = 5;
        private string? _selectedGenre;
        private string _searchTerm = string.Empty;
        private string _statusMessage = "Загрузите список игр.";
        private string _errorMessage = string.Empty;
        private int _topCount = 3;
        private bool _suppressAutoLoad;

        /// <summary>
        /// Инициализирует ViewModel всеми необходимыми сервисами.
        /// </summary>
        /// <param name="logic">Бизнес-логика.</param>
        /// <param name="userInteractionService">Сервис сообщений пользователю.</param>
        /// <param name="fileDialogService">Сервис диалогов выбора файла.</param>
        /// <param name="viewManager">Менеджер отображения View.</param>
        public MainViewModel(
            IGameLogic logic,
            IUserInteractionService userInteractionService,
            IFileDialogService fileDialogService)
        {
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));
            _userInteractionService = userInteractionService ?? throw new ArgumentNullException(nameof(userInteractionService));
            _fileDialogService = fileDialogService ?? throw new ArgumentNullException(nameof(fileDialogService));

            Games = new BindingList<GameDto>();
            Genres = new BindingList<string>();

            LoadCommand = new RelayCommand(_ => LoadData());
            AddCommand = new RelayCommand(_ => AddGame());
            _editCommand = new RelayCommand(_ => EditGame(), _ => SelectedGame != null);
            _deleteCommand = new RelayCommand(_ => DeleteGame(), _ => SelectedGame != null);
            FilterCommand = new RelayCommand(_ => LoadData());
            ResetFilterCommand = new RelayCommand(_ => ResetFilters());
            TopCommand = new RelayCommand(_ => ShowTopGames());
            ExportCommand = new RelayCommand(_ => ExportToJson());
            ImportCommand = new RelayCommand(_ => ImportFromJson());
            ClearFormCommand = new RelayCommand(_ => ClearForm());
        }

        /// <summary>
        /// Коллекция игр для отображения в таблице.
        /// </summary>
        public BindingList<GameDto> Games { get; }

        /// <summary>
        /// Доступные жанры для фильтрации и ввода.
        /// </summary>
        public BindingList<string> Genres { get; }

        /// <summary>
        /// Выбранная игра в таблице.
        /// </summary>
        public GameDto? SelectedGame
        {
            get => _selectedGame;
            set
            {
                if (SetProperty(ref _selectedGame, value))
                {
                    OnGameSelected(value);
                    _editCommand.RaiseCanExecuteChanged();
                    _deleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Название игры из формы.
        /// </summary>
        public string NameInput
        {
            get => _nameInput;
            set => SetProperty(ref _nameInput, value);
        }

        /// <summary>
        /// Жанр из формы.
        /// </summary>
        public string GenreInput
        {
            get => _genreInput;
            set => SetProperty(ref _genreInput, value);
        }

        /// <summary>
        /// Рейтинг из формы.
        /// </summary>
        public double RatingInput
        {
            get => _ratingInput;
            set => SetProperty(ref _ratingInput, value);
        }

        /// <summary>
        /// Выбранный жанр фильтра.
        /// </summary>
        public string? SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                if (SetProperty(ref _selectedGenre, value) && !_suppressAutoLoad)
                {
                    LoadData();
                }
            }
        }

        /// <summary>
        /// Поисковая строка по названию.
        /// </summary>
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (SetProperty(ref _searchTerm, value) && !_suppressAutoLoad)
                {
                    LoadData();
                }
            }
        }

        /// <summary>
        /// Статусное сообщение для пользователя.
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// Количество игр для отбора в топ.
        /// </summary>
        public int TopCount
        {
            get => _topCount;
            set
            {
                if (value < 1)
                {
                    value = 1;
                }

                SetProperty(ref _topCount, value);
            }
        }

        /// <summary>
        /// Команда загрузки данных.
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Команда добавления игры.
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Команда сохранения изменений выбранной игры.
        /// </summary>
        public ICommand EditCommand => _editCommand;

        /// <summary>
        /// Команда удаления выбранной игры.
        /// </summary>
        public ICommand DeleteCommand => _deleteCommand;

        /// <summary>
        /// Команда применения фильтров.
        /// </summary>
        public ICommand FilterCommand { get; }

        /// <summary>
        /// Команда сброса фильтров.
        /// </summary>
        public ICommand ResetFilterCommand { get; }

        /// <summary>
        /// Команда показа топа по рейтингу.
        /// </summary>
        public ICommand TopCommand { get; }

        /// <summary>
        /// Команда экспорта в JSON.
        /// </summary>
        public ICommand ExportCommand { get; }

        /// <summary>
        /// Команда импорта из JSON.
        /// </summary>
        public ICommand ImportCommand { get; }

        /// <summary>
        /// Команда очистки формы.
        /// </summary>
        public ICommand ClearFormCommand { get; }

        /// <summary>
        /// Загружает игры и жанры с учетом фильтров.
        /// </summary>
        public void LoadData()
        {
            try
            {
                ErrorMessage = string.Empty;
                IEnumerable<Models.Game> source = string.IsNullOrWhiteSpace(SelectedGenre)
                    ? _logic.GetAllGames()
                    : _logic.FilterByGenre(SelectedGenre);

                source = ApplySearch(source);
                UpdateGames(source);
                UpdateGenres();
                StatusMessage = $"Найдено записей: {Games.Count}.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                _userInteractionService.ShowError($"Не удалось загрузить данные: {ex.Message}");
            }
        }

        /// <summary>
        /// Сбрасывает фильтры поиска и жанра.
        /// </summary>
        private void ResetFilters()
        {
            _suppressAutoLoad = true;
            SelectedGenre = null;
            SearchTerm = string.Empty;
            _suppressAutoLoad = false;
            LoadData();
        }

        private void AddGame()
        {
            try
            {
                var dto = BuildDtoFromForm();
                if (dto == null)
                {
                    return;
                }

                _logic.AddGame(GameMapper.ToModel(dto));
                _userInteractionService.ShowInfo($"Игра \"{dto.Name}\" добавлена.");
                ClearForm();
                LoadData();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                _userInteractionService.ShowError($"Ошибка при добавлении игры: {ex.Message}");
            }
        }

        private void EditGame()
        {
            if (SelectedGame == null)
            {
                _userInteractionService.ShowError("Сначала выберите игру для редактирования.");
                return;
            }

            try
            {
                var dto = BuildDtoFromForm();
                if (dto == null)
                {
                    return;
                }

                dto.Id = SelectedGame.Id;
                var updated = _logic.UpdateGame(GameMapper.ToModel(dto));
                if (!updated)
                {
                    _userInteractionService.ShowError("Игра не найдена в базе данных.");
                    return;
                }

                _userInteractionService.ShowInfo($"Игра \"{dto.Name}\" обновлена.");
                LoadData();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                _userInteractionService.ShowError($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void DeleteGame()
        {
            if (SelectedGame == null)
            {
                _userInteractionService.ShowError("Сначала выберите игру для удаления.");
                return;
            }

            var confirm = _userInteractionService.Confirm(
                $"Удалить игру \"{SelectedGame.Name}\"?",
                "Удаление записи");

            if (!confirm)
            {
                return;
            }

            try
            {
                var removed = _logic.DeleteGame(SelectedGame.Id);
                if (!removed)
                {
                    _userInteractionService.ShowError("Игра не найдена в базе данных.");
                    return;
                }

                _userInteractionService.ShowInfo("Игра удалена.");
                ClearForm();
                LoadData();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                _userInteractionService.ShowError($"Ошибка при удалении: {ex.Message}");
            }
        }

        private void ShowTopGames()
        {
            try
            {
                if (TopCount < 1)
                {
                    _userInteractionService.ShowError("Количество игр должно быть больше нуля.");
                    return;
                }

                var top = _logic.GetTopRatedGames(TopCount);
                UpdateGames(top);
                StatusMessage = $"Топ {TopCount} игр по рейтингу.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                _userInteractionService.ShowError($"Не удалось показать топ: {ex.Message}");
            }
        }

        private void ExportToJson()
        {
            var path = _fileDialogService.AskSaveFilePath("games_export.json", "JSON (*.json)|*.json|Все файлы (*.*)|*.*");
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                _logic.ExportToJson(path);
                _userInteractionService.ShowInfo($"Данные сохранены в файл: {path}");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                _userInteractionService.ShowError($"Экспорт не выполнен: {ex.Message}");
            }
        }

        private void ImportFromJson()
        {
            var path = _fileDialogService.AskOpenFilePath("JSON (*.json)|*.json|Все файлы (*.*)|*.*");
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                var added = _logic.ImportFromJson(path);
                _userInteractionService.ShowInfo($"Импорт завершен. Добавлено записей: {added}.");
                LoadData();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                _userInteractionService.ShowError($"Импорт не выполнен: {ex.Message}");
            }
        }

        private void OnGameSelected(GameDto? dto)
        {
            if (dto == null)
            {
                ClearForm();
                return;
            }

            NameInput = dto.Name;
            GenreInput = dto.Genre;
            RatingInput = dto.Rating;
        }

        private void UpdateGames(IEnumerable<Models.Game> source)
        {
            Games.RaiseListChangedEvents = false;
            Games.Clear();
            foreach (var game in source)
            {
                Games.Add(GameMapper.FromModel(game));
            }

            Games.RaiseListChangedEvents = true;
            Games.ResetBindings();

            if (SelectedGame != null)
            {
                SelectedGame = Games.FirstOrDefault(g => g.Id == SelectedGame.Id);
            }
        }

        private void UpdateGenres()
        {
            var genres = _logic.GetAllGames()
                .Select(g => g.Genre)
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(g => g)
                .ToList();

            Genres.RaiseListChangedEvents = false;
            Genres.Clear();
            foreach (var genre in genres)
            {
                Genres.Add(genre);
            }

            Genres.RaiseListChangedEvents = true;
            Genres.ResetBindings();
        }

        private IEnumerable<Models.Game> ApplySearch(IEnumerable<Models.Game> source)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                return source;
            }

            return source.Where(g => g.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        private GameDto? BuildDtoFromForm()
        {
            ErrorMessage = string.Empty;
            var name = NameInput?.Trim() ?? string.Empty;
            var genre = GenreInput?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                ErrorMessage = "Введите название игры.";
                _userInteractionService.ShowError(ErrorMessage);
                return null;
            }

            if (string.IsNullOrWhiteSpace(genre))
            {
                ErrorMessage = "Выберите или введите жанр.";
                _userInteractionService.ShowError(ErrorMessage);
                return null;
            }

            if (RatingInput is < 0 or > 10)
            {
                ErrorMessage = "Рейтинг должен быть в диапазоне 0..10.";
                _userInteractionService.ShowError(ErrorMessage);
                return null;
            }

            return new GameDto
            {
                Id = 0,
                Name = name,
                Genre = genre,
                Rating = RatingInput
            };
        }

        private void ClearForm()
        {
            NameInput = string.Empty;
            GenreInput = string.Empty;
            RatingInput = 5;
            SelectedGame = null;
        }
    }
}
