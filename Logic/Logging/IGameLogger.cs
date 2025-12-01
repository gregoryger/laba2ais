using Models;

namespace Logic.Logging
{
    /// <summary>
    /// Логирование операций с играми.
    /// </summary>
    public interface IGameLogger
    {
        /// <summary>
        /// Записывает информационное сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void LogInfo(string message);

        /// <summary>
        /// Записывает ошибку с деталями.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="exceptionMessage">Описание ошибки.</param>
        void LogError(string message, string exceptionMessage);

        /// <summary>
        /// Фиксирует снимок состояния игры.
        /// </summary>
        /// <param name="game">Игра для логирования.</param>
        void LogGameSnapshot(Game game);
    }
}
