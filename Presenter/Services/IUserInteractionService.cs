namespace GameApp.Presenter.Services
{
    /// <summary>
    /// Определяет сервис взаимодействия с пользователем, реализуемый слоем View.
    /// </summary>
    public interface IUserInteractionService
    {
        /// <summary>
        /// Показывает информационное сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void ShowInfo(string message);

        /// <summary>
        /// Показывает сообщение об ошибке.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void ShowError(string message);

        /// <summary>
        /// Запрашивает подтверждение действия.
        /// </summary>
        /// <param name="message">Текст вопроса.</param>
        /// <param name="caption">Заголовок окна.</param>
        /// <returns>True, если пользователь подтвердил.</returns>
        bool Confirm(string message, string caption);
    }
}
