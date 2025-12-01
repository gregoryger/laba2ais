using System;
using System.Linq;
using GameApp.Shared;
using Logic;
using Models;

namespace GameApp.Presenter
{
    /// <summary>
    /// Основной Presenter, координирующий View и бизнес-логику.
    /// </summary>
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IGameLogic _logic;

        /// <summary>
        /// Создает Presenter.
        /// </summary>
        /// <param name="view">Экземпляр представления.</param>
        /// <param name="logic">Слой бизнес-логики.</param>
        public MainPresenter(IMainView view, IGameLogic logic)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));
        }

        /// <summary>
        /// Настраивает подписки и выполняет начальную загрузку данных.
        /// </summary>
        public void Initialize()
        {
            _view.LoadRequested += OnLoadRequested;
            _view.AddGameRequested += OnAddGameRequested;
            _view.EditGameRequested += OnEditGameRequested;
            _view.DeleteGameRequested += OnDeleteGameRequested;
            _view.FilterRequested += OnFilterRequested;
            _view.TopGamesRequested += OnTopGamesRequested;
            _view.ExportRequested += OnExportRequested;
            _view.ImportRequested += OnImportRequested;
            _view.SearchRequested += OnSearchRequested;

            RefreshData();
        }

        private void OnLoadRequested(object? sender, EventArgs e)
        {
            RefreshData();
        }

        private void OnAddGameRequested(object? sender, EventArgs e)
        {
            var draft = _view.RequestGameData(null);
            if (draft == null)
            {
                return;
            }

            try
            {
                _logic.AddGame(ToModel(draft));
                RefreshData();
                _view.ShowInfo("Игра добавлена.");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Не удалось добавить игру: {ex.Message}");
            }
        }

        private void OnEditGameRequested(object? sender, GameEventArgs e)
        {
            var updated = _view.RequestGameData(e.Game);
            if (updated == null)
            {
                return;
            }

            try
            {
                var isUpdated = _logic.UpdateGame(ToModel(updated));
                if (isUpdated)
                {
                    RefreshData();
                    _view.ShowInfo("Игра обновлена.");
                }
                else
                {
                    _view.ShowError("Игра не найдена.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Не удалось обновить игру: {ex.Message}");
            }
        }

        private void OnDeleteGameRequested(object? sender, GameEventArgs e)
        {
            if (!_view.Confirm($"Удалить игру \"{e.Game.Name}\"?", "Подтверждение удаления"))
            {
                return;
            }

            try
            {
                var deleted = _logic.DeleteGame(e.Game.Id);
                if (deleted)
                {
                    RefreshData();
                    _view.ShowInfo("Игра удалена.");
                }
                else
                {
                    _view.ShowError("Игра не найдена.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Не удалось удалить игру: {ex.Message}");
            }
        }

        private void OnFilterRequested(object? sender, GenreFilterEventArgs e)
        {
            RefreshData(string.IsNullOrWhiteSpace(e.Genre) ? null : e.Genre, null);
        }

        private void OnTopGamesRequested(object? sender, TopGamesRequestEventArgs e)
        {
            try
            {
                var games = _logic.GetTopRatedGames(e.Count).Select(ToViewModel).ToList();
                _view.ShowGames(games);
                _view.ShowInfo($"Показано {games.Count} игр с наивысшим рейтингом.");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Не удалось получить топ игр: {ex.Message}");
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
                var added = _logic.ImportFromJson(e.Path);
                RefreshData();
                _view.ShowInfo($"Импорт завершен. Добавлено записей: {added}.");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при импорте: {ex.Message}");
            }
        }

        private void OnSearchRequested(object? sender, string e)
        {
            RefreshData(null, e);
        }

        private void RefreshData(string? genre = null, string? namePart = null)
        {
            try
            {
                var games = string.IsNullOrWhiteSpace(genre)
                    ? _logic.GetAllGames()
                    : _logic.FilterByGenre(genre);

                if (!string.IsNullOrWhiteSpace(namePart))
                {
                    games = games
                        .Where(g => g.Name.Contains(namePart, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var viewModels = games.Select(ToViewModel).ToList();
                var genres = _logic.GetAllGames()
                    .Select(g => g.Genre)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(g => g)
                    .ToList();

                _view.ShowGames(viewModels);
                _view.ShowGenres(genres, genre);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Не удалось загрузить данные: {ex.Message}");
            }
        }

        private static GameViewModel ToViewModel(Game game)
        {
            return new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Genre = game.Genre,
                Rating = game.Rating
            };
        }

        private static Game ToModel(GameViewModel viewModel)
        {
            return new Game
            {
                Id = viewModel.Id,
                Name = viewModel.Name.Trim(),
                Genre = viewModel.Genre.Trim(),
                Rating = viewModel.Rating
            };
        }
    }
}
