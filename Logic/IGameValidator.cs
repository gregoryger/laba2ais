using Models;

namespace Logic
{
    /// <summary>
    /// Валидация данных о сущности Game.
    /// </summary>
    public interface IGameValidator
    {
        void Validate(Game game);

        void ValidateId(int id);
    }
}
