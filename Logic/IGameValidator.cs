using Models;

namespace Logic
{
    /// <summary>
    /// Контракт валидации данных игры.
    /// </summary>
    public interface IGameValidator
    {
        /// <summary>
        /// Проверяет корректность данных игры.
        /// </summary>
        /// <param name="game">Проверяемая игра.</param>
        void Validate(Game game);

        /// <summary>
        /// Проверяет идентификатор на корректность.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        void ValidateId(int id);
    }
}
