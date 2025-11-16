using Models;

namespace Logic.Logging
{
    /// <summary>
    /// Обеспечивает аудит действий над играми.
    /// </summary>
    public interface IGameLogger
    {
        /// <summary>
        /// Логирует успешное действие.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void LogInfo(string message);

        /// <summary>
        /// Логирует неуспешное действие и причину.
        /// </summary>
        /// <param name="message">Контекст сообщения.</param>
        /// <param name="exceptionMessage">Детали исключения.</param>
        void LogError(string message, string exceptionMessage);

        /// <summary>
        /// Логирует краткую информацию об игре.
        /// </summary>
        /// <param name="game">Игра для описания.</param>
        void LogGameSnapshot(Game game);
    }
}
