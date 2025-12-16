using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameApp.Presenter.ViewModels
{
    /// <summary>
    /// Базовый класс ViewModel с поддержкой уведомлений об изменении свойств.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие, возникающее при изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Устанавливает значение поля и генерирует уведомление, если значение изменилось.
        /// </summary>
        /// <typeparam name="T">Тип свойства.</typeparam>
        /// <param name="field">Ссылка на поле.</param>
        /// <param name="value">Новое значение.</param>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>True, если значение изменилось.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Генерирует уведомление об изменении свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Выполняет действие с проверкой на null.
        /// </summary>
        /// <param name="action">Действие для выполнения.</param>
        /// <exception cref="ArgumentNullException">Если action равен null.</exception>
        protected static void GuardedAction(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            action();
        }
    }
}
