using System;
using Models;

namespace Logic
{
    /// <summary>
    /// Простая реализация проверок для сущности Game.
    /// </summary>
    public class GameValidator : IGameValidator
    {
        public void Validate(Game game)
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

        public void ValidateId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Identifier must be a positive number.", nameof(id));
            }
        }
    }
}
