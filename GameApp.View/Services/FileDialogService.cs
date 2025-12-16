using System.Windows;
using GameApp.Presenter.Services;
using Microsoft.Win32;

namespace GameApp.View.Services
{
    /// <summary>
    /// Сервис выбора файлов для импорта и экспорта.
    /// </summary>
    public class FileDialogService : IFileDialogService
    {
        /// <inheritdoc />
        public string? AskSaveFilePath(string defaultFileName, string filter)
        {
            var dialog = new SaveFileDialog
            {
                Filter = filter,
                FileName = defaultFileName
            };

            return dialog.ShowDialog(Application.Current.MainWindow) == true ? dialog.FileName : null;
        }

        /// <inheritdoc />
        public string? AskOpenFilePath(string filter)
        {
            var dialog = new OpenFileDialog
            {
                Filter = filter
            };

            return dialog.ShowDialog(Application.Current.MainWindow) == true ? dialog.FileName : null;
        }
    }
}
