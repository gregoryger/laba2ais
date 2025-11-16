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

        /// <summary>
        /// Создаёт логику и подключает зависимости.
        /// </summary>
        /// <param name="repository">Абстракция репозитория.</param>
        /// <param name="logger">Логгер для аудита.</param>
        public GameLogic(IRepository<Game> repository, IGameLogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public Game AddGame(Game game)
        {
            ValidateGame(game);

            _repository.Add(game);
            _logger.LogInfo($"Game added: {game.Name} ({game.Genre}) with rating {game.Rating}.");
            _logger.LogGameSnapshot(game);

            return game;
        }

        /// <inheritdoc/>
        public bool DeleteGame(int id)
        {
            ValidateId(id);

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
            ValidateId(id);
            return _repository.ReadById(id);
        }

        /// <inheritdoc/>
        public bool UpdateGame(Game game)
        {
            ValidateGame(game);
            ValidateId(game.Id);

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

        private static void ValidateGame(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            if (string.IsNullOrWhiteSpace(game.Name))
            {
                throw new ArgumentException("Game name must be provided.", nameof(game));
            }

            if (string.IsNullOrWhiteSpace(game.Genre))
            {
                throw new ArgumentException("Game genre must be provided.", nameof(game));
            }

            if (game.Rating < 0 || game.Rating > 10)
            {
                throw new ArgumentException("Rating must be between 0 and 10.", nameof(game));
            }
        }

        private static void ValidateId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Identifier must be a positive number.", nameof(id));
            }
        }
    }
}
