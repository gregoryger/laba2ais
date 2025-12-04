using System;
using System.Collections.Generic;
using System.Linq;
using GameApp.Shared;
using Logic;
using Models;

namespace GameApp.Presenter
{
    /// <summary>
    /// Presenter в архитектуре MVP: подписывается на события View, вызывает бизнес-логику и обновляет View.
    /// Работает только через интерфейсы IMainView и IGameLogic.
    /// </summary>
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IGameLogic _logic;

        /// <summary>
        /// Создаёт Presenter и сразу подписывается на события View и модели.
        /// </summary>
        /// <param name="view">Экземпляр представления, реализующий IMainView.</param>
        /// <param name="logic">Слой бизнес-логики для работы с играми.</param>
        public MainPresenter(IMainView view, IGameLogic logic)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));

            _view.LoadRequested += OnLoadRequested;
            _view.AddGameRequested += OnAddGameRequested;
            _view.EditGameRequested += OnEditGameRequested;
            _view.DeleteGameRequested += OnDeleteGameRequested;
            _view.FilterRequested += OnFilterRequested;
            _view.TopGamesRequested += OnTopGamesRequested;
            _view.ExportRequested += OnExportRequested;
            _view.ImportRequested += OnImportRequested;
            _view.SearchRequested += OnSearchRequested;

            _logic.DataChanged += (_, _) => LoadAndShow();
            _logic.GameAdded += (_, args) => _view.ShowInfo($"Добавлена игра: {args.Game.Name}");
            _logic.GameUpdated += (_, args) => _view.ShowInfo($"Обновлена игра: {args.Game.Name}");
            _logic.GameDeleted += (_, args) => _view.ShowInfo($"Удалена игра: {args.Game.Name}");
            _logic.GamesImported += (_, args) => _view.ShowInfo($"Импорт завершен. Добавлено записей: {args.AddedCount}");
        }

        private string? _currentGenre;
        private string? _searchTerm;

        private void OnLoadRequested(object? sender, EventArgs e)
        {
            LoadAndShow();
        }

        private void OnAddGameRequested(object? sender, EventArgs e)
        {
            try
            {
                var draft = _view.RequestGameData(null);
                if (draft == null)
                {
                    return;
                }

                _logic.AddGame(ToDomain(draft));
                // DataChanged событие обновит View.
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при добавлении игры: {ex.Message}");
            }
        }

        private void OnEditGameRequested(object? sender, GameEventArgs e)
        {
            try
            {
                var updated = _view.RequestGameData(e.Game);
                if (updated == null)
                {
                    return;
                }

                var ok = _logic.UpdateGame(ToDomain(updated));
                if (!ok)
                {
                    _view.ShowError("Игра не найдена.");
                }
                // DataChanged событие обновит View.
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при обновлении игры: {ex.Message}");
            }
        }

        private void OnDeleteGameRequested(object? sender, GameEventArgs e)
        {
            try
            {
                var ok = _logic.DeleteGame(e.Game.Id);
                if (!ok)
                {
                    _view.ShowError("Игра не найдена.");
                }
                // DataChanged событие обновит View.
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при удалении игры: {ex.Message}");
            }
        }

        private void OnFilterRequested(object? sender, GenreFilterEventArgs e)
        {
            _currentGenre = string.IsNullOrWhiteSpace(e.Genre) ? null : e.Genre;
            LoadAndShow();
        }

        private void OnSearchRequested(object? sender, string term)
        {
            _searchTerm = string.IsNullOrWhiteSpace(term) ? null : term;
            LoadAndShow();
        }

        private void OnTopGamesRequested(object? sender, TopGamesRequestEventArgs e)
        {
            try
            {
                var top = _logic.GetTopRatedGames(e.Count).Select(ToViewModel).ToList();
                _view.ShowGames(top);
                _view.ShowInfo($"Показано {top.Count} игр с наивысшим рейтингом.");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при получении топа: {ex.Message}");
            }
        }

        private void OnExportRequested(object? sender, FilePathEventArgs e)
        {
            try
            {
                _logic.ExportToJson(e.Path);
                _view.ShowInfo($"Экспорт выполнен: {e.Path}");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при экспорте: {ex.Message}");
            }
        }

        private void OnImportRequested(object? sender, FilePathEventArgs e)
        {
            try
            {
                _logic.ImportFromJson(e.Path);
                // DataChanged событие обновит View и уведомит пользователя.
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при импорте: {ex.Message}");
            }
        }

        private void LoadAndShow()
        {
            try
            {
                var games = LoadFilteredGames();
                var viewModels = games.Select(ToViewModel).ToList();
                var genres = _logic.GetAllGames()
                    .Select(g => g.Genre)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(g => g)
                    .ToList();

                _view.ShowGames(viewModels);
                _view.ShowGenres(genres, _currentGenre);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Не удалось обновить данные: {ex.Message}");
            }
        }

        private List<Game> LoadFilteredGames()
        {
            var games = string.IsNullOrWhiteSpace(_currentGenre)
                ? _logic.GetAllGames()
                : _logic.FilterByGenre(_currentGenre);

            if (!string.IsNullOrWhiteSpace(_searchTerm))
            {
                games = games
                    .Where(g => g.Name.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return games;
        }

        private static GameViewModel ToViewModel(Game game) =>
            new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Genre = game.Genre,
                Rating = game.Rating
            };

        private static Game ToDomain(GameViewModel vm) =>
            new Game
            {
                Id = vm.Id,
                Name = vm.Name.Trim(),
                Genre = vm.Genre.Trim(),
                Rating = vm.Rating
            };
    }
}
