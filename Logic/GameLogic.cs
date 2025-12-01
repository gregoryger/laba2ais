using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using DataAccessLayer;
using Logic.Logging;
using Models;

namespace Logic
{
    /// <summary>
    /// Реализация бизнес-логики для работы с играми.
    /// </summary>
    public class GameLogic : IGameLogic
    {
        private readonly IRepository<Game> _repository;
        private readonly IGameLogger _logger;
        private readonly IGameValidator _validator;

        /// <summary>
        /// Создает экземпляр слоя бизнес-логики.
        /// </summary>
        /// <param name="repository">Репозиторий игр.</param>
        /// <param name="logger">Логгер операций.</param>
        /// <param name="validator">Валидатор данных игры.</param>
        public GameLogic(IRepository<Game> repository, IGameLogger logger, IGameValidator validator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <inheritdoc/>
        public Game AddGame(Game game)
        {
            _validator.Validate(game);
            _repository.Add(game);
            _logger.LogInfo($"Добавлена игра: {game.Name} ({game.Genre}) с рейтингом {game.Rating:0.0}");
            _logger.LogGameSnapshot(game);
            return game;
        }

        /// <inheritdoc/>
        public bool DeleteGame(int id)
        {
            _validator.ValidateId(id);
            var game = _repository.ReadById(id);
            if (game == null)
            {
                return false;
            }

            _repository.Delete(id);
            _logger.LogInfo($"Удалена игра: {game.Name} (Id={id}).");
            return true;
        }

        /// <inheritdoc/>
        public List<Game> GetAllGames()
        {
            return _repository.ReadAll().Select(Clone).ToList();
        }

        /// <inheritdoc/>
        public Game? GetGameById(int id)
        {
            _validator.ValidateId(id);
            var game = _repository.ReadById(id);
            return game == null ? null : Clone(game);
        }

        /// <inheritdoc/>
        public bool UpdateGame(Game game)
        {
            _validator.Validate(game);
            _validator.ValidateId(game.Id);

            var existingGame = _repository.ReadById(game.Id);
            if (existingGame == null)
            {
                return false;
            }

            _repository.Update(game);
            _logger.LogInfo($"Обновлена игра: {game.Name} (Id={game.Id}).");
            _logger.LogGameSnapshot(game);
            return true;
        }

        /// <inheritdoc/>
        public List<Game> FilterByGenre(string genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
            {
                return _repository.ReadAll().Select(Clone).ToList();
            }

            return _repository.ReadAll()
                .Where(g => g.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                .Select(Clone)
                .ToList();
        }

        /// <inheritdoc/>
        public List<Game> GetTopRatedGames(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Количество должно быть не меньше 1.");
            }

            return _repository.ReadAll()
                .OrderByDescending(g => g.Rating)
                .Take(count)
                .Select(Clone)
                .ToList();
        }

        /// <inheritdoc/>
        public void ExportToJson(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Путь к файлу обязателен.", nameof(filePath));
            }

            var games = _repository.ReadAll().Select(Clone).ToList();
            var json = JsonSerializer.Serialize(
                games,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
            File.WriteAllText(filePath, json);
            _logger.LogInfo($"Экспортировано {games.Count} игра(игр) в файл {filePath}");
        }

        /// <inheritdoc/>
        public int ImportFromJson(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Путь к файлу обязателен.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл не найден.", filePath);
            }

            var content = File.ReadAllText(filePath);
            var imported = JsonSerializer.Deserialize<List<Game>>(content) ?? new List<Game>();
            var added = 0;
            var existing = _repository.ReadAll();

            foreach (var game in imported)
            {
                _validator.Validate(game);
                game.Id = 0;
                var duplicate = existing.Any(g => g.Name.Equals(game.Name, StringComparison.OrdinalIgnoreCase)
                                                  && g.Genre.Equals(game.Genre, StringComparison.OrdinalIgnoreCase));
                if (duplicate)
                {
                    continue;
                }

                _repository.Add(game);
                existing.Add(game);
                added++;
            }

            _logger.LogInfo($"Импорт завершен. Добавлено: {added} записей из {filePath}.");
            return added;
        }

        private static Game Clone(Game source)
        {
            return new Game
            {
                Id = source.Id,
                Name = source.Name,
                Genre = source.Genre,
                Rating = source.Rating
            };
        }
    }
}
