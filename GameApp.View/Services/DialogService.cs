using System.Windows;
using GameApp.Presenter.Services;

namespace GameApp.View.Services
{
    /// <summary>
    /// WPF-реализация сервиса взаимодействия с пользователем через MessageBox.
    /// </summary>
    public class DialogService : IUserInteractionService
    {
        /// <inheritdoc />
        public void ShowInfo(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <inheritdoc />
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <inheritdoc />
        public bool Confirm(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
    }
}
