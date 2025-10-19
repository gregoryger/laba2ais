using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using DataAccessLayer;

namespace Logic
{
    /// <summary>
    /// Класс бизнес-логики для работы с сущностями Game (CRUD-операции и специальные функции).
    /// </summary>
    public class GameLogic
    {
        private readonly IRepository<Game> _repository;

        /// <summary>
        /// Конструктор класса бизнес-логики.
        /// </summary>
        /// <param name="repository">Репозиторий для работы с играми.</param>
        public GameLogic(IRepository<Game> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Добавляет новую игру в систему.
        /// </summary>
        /// <param name="game">Новая игра для добавления.</param>
        /// <returns>Добавленная игра с присвоенным идентификатором.</returns>
        public Game AddGame(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            if (string.IsNullOrWhiteSpace(game.Name))
                throw new ArgumentException("Название игры не может быть пустым.", nameof(game));

            if (string.IsNullOrWhiteSpace(game.Genre))
                throw new ArgumentException("Жанр игры не может быть пустым.", nameof(game));

            if (game.Rating < 0 || game.Rating > 10)
                throw new ArgumentException("Рейтинг должен быть в диапазоне от 0 до 10.", nameof(game));

            _repository.Add(game);
            return game;
        }

        /// <summary>
        /// Удаляет игру по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор игры для удаления.</param>
        /// <returns>True, если игра найдена и удалена, иначе False.</returns>
        public bool DeleteGame(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID должен быть положительным числом.", nameof(id));

            var game = _repository.ReadById(id);
            if (game == null)
                return false;

            _repository.Delete(id);
            return true;
        }

        /// <summary>
        /// Возвращает список всех игр.
        /// </summary>
        /// <returns>Список всех игр.</returns>
        public List<Game> GetAllGames()
        {
            return _repository.ReadAll();
        }

        /// <summary>
        /// Находит игру по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомой игры.</param>
        /// <returns>Игру с заданным Id, либо null, если не найдена.</returns>
        public Game GetGameById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID должен быть положительным числом.", nameof(id));

            return _repository.ReadById(id);
        }

        /// <summary>
        /// Обновляет данные существующей игры.
        /// </summary>
        /// <param name="game">Обновлённая игра (должна иметь корректный Id).</param>
        /// <returns>True, если игра найдена и обновлена, иначе False.</returns>
        public bool UpdateGame(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            if (string.IsNullOrWhiteSpace(game.Name))
                throw new ArgumentException("Название игры не может быть пустым.", nameof(game));

            if (string.IsNullOrWhiteSpace(game.Genre))
                throw new ArgumentException("Жанр игры не может быть пустым.", nameof(game));

            if (game.Rating < 0 || game.Rating > 10)
                throw new ArgumentException("Рейтинг должен быть в диапазоне от 0 до 10.", nameof(game));

            var existingGame = _repository.ReadById(game.Id);
            if (existingGame == null)
                return false;

            _repository.Update(game);
            return true;
        }

        /// <summary>
        /// Фильтрует список игр по указанному жанру (без учёта регистра).
        /// </summary>
        /// <param name="genre">Жанр для фильтрации.</param>
        /// <returns>Список игр указанного жанра.</returns>
        public List<Game> FilterByGenre(string genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
                throw new ArgumentException("Жанр не может быть пустым.", nameof(genre));

            return _repository.ReadAll()
                .Where(g => g.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Возвращает топ N игр по убыванию рейтинга.
        /// </summary>
        /// <param name="count">Максимальное число игр в топе.</param>
        /// <returns>Список игр с наивысшими рейтингами (не более count штук).</returns>
        public List<Game> GetTopRatedGames(int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), "Количество должно быть больше нуля.");

            return _repository.ReadAll()
                .OrderByDescending(g => g.Rating)
                .Take(count)
                .ToList();
        }
    }
}
