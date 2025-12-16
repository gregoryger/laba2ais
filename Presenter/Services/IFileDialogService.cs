namespace GameApp.Presenter.Services
{
    /// <summary>
    /// Абстракция для выбора файлов.
    /// </summary>
    public interface IFileDialogService
    {
        /// <summary>
        /// Запрашивает путь для сохранения файла.
        /// </summary>
        /// <param name="defaultFileName">Предлагаемое имя файла.</param>
        /// <param name="filter">Строка фильтра.</param>
        /// <returns>Выбранный путь или null при отмене.</returns>
        string? AskSaveFilePath(string defaultFileName, string filter);

        /// <summary>
        /// Запрашивает путь к файлу для открытия.
        /// </summary>
        /// <param name="filter">Строка фильтра.</param>
        /// <returns>Выбранный путь или null при отмене.</returns>
        string? AskOpenFilePath(string filter);
    }
}
