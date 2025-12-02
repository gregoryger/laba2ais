using System;
using System.Collections.Generic;
using System.Linq;
using GameApp.Shared;
using Logic;
using Models;

namespace GameApp.Presenter
{
    /// <summary>
    /// Presenter, который оркестрирует взаимодействие View и бизнес-логики.
    /// </summary>
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IGameLogic _logic;
        private readonly FilterState _filter = new();

        /// <summary>
        /// Создает Presenter.
        /// </summary>
        /// <param name="view">Реализация интерфейса представления.</param>
        /// <param name="logic">Слой бизнес-логики.</param>
        public MainPresenter(IMainView view, IGameLogic logic)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));
        }

        /// <summary>
        /// Подписывает события View и выполняет начальную загрузку.
        /// </summary>
        public void Initialize()
        {
            WireView();
            RefreshView();
        }

        private void WireView()
        {
            _view.LoadRequested += (_, _) => RefreshView();
            _view.AddGameRequested += (_, _) => AddGame();
            _view.EditGameRequested += (_, args) => EditGame(args.Game);
            _view.DeleteGameRequested += (_, args) => DeleteGame(args.Game);
            _view.FilterRequested += (_, args) => ApplyGenreFilter(args.Genre);
            _view.TopGamesRequested += (_, args) => ShowTop(args.Count);
            _view.ExportRequested += (_, args) => Export(args.Path);
            _view.ImportRequested += (_, args) => Import(args.Path);
            _view.SearchRequested += (_, term) => ApplySearch(term);
        }

        private void AddGame()
        {
            var draft = _view.RequestGameData(null);
            if (draft == null)
            {
                return;
            }

            TryExecute(
                () =>
                {
                    _logic.AddGame(ToDomain(draft));
                    RefreshView();
                    _view.ShowInfo("Игра добавлена.");
                },
                "Не удалось добавить игру");
        }

        private void EditGame(GameViewModel game)
        {
            var updated = _view.RequestGameData(game);
            if (updated == null)
            {
                return;
            }

            TryExecute(
                () =>
                {
                    var ok = _logic.UpdateGame(ToDomain(updated));
                    if (!ok)
                    {
                        _view.ShowError("Игра не найдена.");
                        return;
                    }

                    RefreshView();
                    _view.ShowInfo("Игра обновлена.");
                },
                "Не удалось обновить игру");
        }

        private void DeleteGame(GameViewModel game)
        {
            if (!_view.Confirm($"Удалить игру \"{game.Name}\"?", "Подтверждение удаления"))
            {
                return;
            }

            TryExecute(
                () =>
                {
                    var ok = _logic.DeleteGame(game.Id);
                    if (!ok)
                    {
                        _view.ShowError("Игра не найдена.");
                        return;
                    }

                    RefreshView();
                    _view.ShowInfo("Игра удалена.");
                },
                "Не удалось удалить игру");
        }

        private void ApplyGenreFilter(string genre)
        {
            _filter.Genre = string.IsNullOrWhiteSpace(genre) ? null : genre;
            RefreshView();
        }

        private void ApplySearch(string searchTerm)
        {
            _filter.NamePart = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm;
            RefreshView();
        }

        private void ShowTop(int count)
        {
            TryExecute(
                () =>
                {
                    var games = _logic.GetTopRatedGames(count)
                        .Select(ToViewModel)
                        .ToList();
                    _view.ShowGames(games);
                    _view.ShowInfo($"Показано {games.Count} лучших игр.");
                },
                "Не удалось получить топ игр");
        }

        private void Export(string path)
        {
            TryExecute(
                () =>
                {
                    _logic.ExportToJson(path);
                    _view.ShowInfo($"Экспорт выполнен: {path}");
                },
                "Ошибка при экспорте");
        }

        private void Import(string path)
        {
            TryExecute(
                () =>
                {
                    var added = _logic.ImportFromJson(path);
                    RefreshView();
                    _view.ShowInfo($"Импорт завершен. Добавлено записей: {added}.");
                },
                "Ошибка при импорте");
        }

        private void RefreshView()
        {
            TryExecute(
                () =>
                {
                    var games = LoadGames(_filter);
                    var viewModels = games.Select(ToViewModel).ToList();
                    var genres = _logic.GetAllGames()
                        .Select(g => g.Genre)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(g => g)
                        .ToList();

                    _view.ShowGames(viewModels);
                    _view.ShowGenres(genres, _filter.Genre);
                },
                "Не удалось обновить данные");
        }

        private List<Game> LoadGames(FilterState filter)
        {
            var games = string.IsNullOrWhiteSpace(filter.Genre)
                ? _logic.GetAllGames()
                : _logic.FilterByGenre(filter.Genre);

            if (!string.IsNullOrWhiteSpace(filter.NamePart))
            {
                games = games
                    .Where(g => g.Name.Contains(filter.NamePart, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return games;
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

        private static Game ToDomain(GameViewModel viewModel)
        {
            return new Game
            {
                Id = viewModel.Id,
                Name = viewModel.Name.Trim(),
                Genre = viewModel.Genre.Trim(),
                Rating = viewModel.Rating
            };
        }

        private void TryExecute(Action action, string errorContext)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _view.ShowError($"{errorContext}: {ex.Message}");
            }
        }

        private class FilterState
        {
            public string? Genre { get; set; }
            public string? NamePart { get; set; }
        }
    }
}
