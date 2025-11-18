using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataAccessLayer;
using Logic.Logging;
using Models;

namespace Logic
{
    /// <summary>
    /// Реализует сценарии работы с сущностью <see cref="Game"/>.
    /// </summary>
    public class GameLogic : IGameLogic
    {
        private readonly IRepository<Game> _repository;
        private readonly IGameLogger _logger;
        private readonly IGameValidator _validator;

        /// <summary>
        /// Создаёт логику и подключает зависимости.
        /// </summary>
        /// <param name="repository">Абстракция репозитория.</param>
        /// <param name="logger">Логгер для аудита.</param>
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
            _logger.LogInfo($"Game added: {game.Name} ({game.Genre}) with rating {game.Rating}.");
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
            _logger.LogInfo($"Game deleted: {game.Name} (Id={id}).");
            return true;
        }

        /// <inheritdoc/>
        public List<Game> GetAllGames()
        {
            return _repository.ReadAll();
        }

        /// <inheritdoc/>
        public Game? GetGameById(int id)
        {
            _validator.ValidateId(id);
            return _repository.ReadById(id);
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
            _logger.LogInfo($"Game updated: {game.Name} (Id={game.Id}).");
            _logger.LogGameSnapshot(game);
            return true;
        }

        /// <inheritdoc/>
        public List<Game> FilterByGenre(string genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
            {
                throw new ArgumentException("Genre is required.", nameof(genre));
            }

            return _repository.ReadAll()
                .Where(g => g.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <inheritdoc/>
        public List<Game> GetTopRatedGames(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be at least 1.");
            }

            return _repository.ReadAll()
                .OrderByDescending(g => g.Rating)
                .Take(count)
                .ToList();
        }

        /// <inheritdoc/>
        public void ExportToJson(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path must not be empty.", nameof(filePath));
            }

            var games = _repository.ReadAll();
            var json = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(filePath, json);
            _logger.LogInfo($"Exported {games.Count} game(s) to \"{filePath}\".");
        }

    }
}
