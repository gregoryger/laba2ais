using System;
using Models;

namespace GameApp.Presenter.Dto
{
    /// <summary>
    /// Утилиты для явного маппинга между моделями и DTO.
    /// </summary>
    public static class GameMapper
    {
        /// <summary>
        /// Создает DTO на основе доменной модели.
        /// </summary>
        /// <param name="model">Экземпляр доменной модели.</param>
        /// <returns>DTO с копией данных.</returns>
        /// <exception cref="ArgumentNullException">Если model равен null.</exception>
        public static GameDto FromModel(Game model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new GameDto
            {
                Id = model.Id,
                Name = model.Name,
                Genre = model.Genre,
                Rating = model.Rating
            };
        }

        /// <summary>
        /// Преобразует DTO в доменную модель.
        /// </summary>
        /// <param name="dto">DTO с данными пользователя.</param>
        /// <returns>Доменная модель с заполненными полями.</returns>
        /// <exception cref="ArgumentNullException">Если dto равен null.</exception>
        public static Game ToModel(GameDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            return new Game
            {
                Id = dto.Id,
                Name = dto.Name.Trim(),
                Genre = dto.Genre.Trim(),
                Rating = dto.Rating
            };
        }
    }
}
