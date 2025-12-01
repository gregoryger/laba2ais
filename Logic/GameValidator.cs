using System;
using Models;

namespace Logic
{
    /// <summary>
    /// Валидатор данных игры.
    /// </summary>
    public class GameValidator : IGameValidator
    {
        /// <inheritdoc/>
        public void Validate(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            if (string.IsNullOrWhiteSpace(game.Name))
            {
                throw new ArgumentException("Название игры обязательно.", nameof(game));
            }

            if (string.IsNullOrWhiteSpace(game.Genre))
            {
                throw new ArgumentException("Жанр игры обязателен.", nameof(game));
            }

            if (game.Rating < 0 || game.Rating > 10)
            {
                throw new ArgumentException("Рейтинг должен быть в диапазоне 0..10.", nameof(game));
            }
        }

        /// <inheritdoc/>
        public void ValidateId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Идентификатор должен быть положительным.", nameof(id));
            }
        }
    }
}
