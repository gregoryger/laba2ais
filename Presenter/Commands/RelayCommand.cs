using System;
using System.Windows.Input;

namespace GameApp.Presenter.Commands
{
    /// <summary>
    /// Простая реализация ICommand для привязки команд во ViewModel.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _executeAction;
        private readonly Predicate<object?>? _canExecutePredicate;

        /// <summary>
        /// Создает команду с обязательным действием и опциональным предикатом доступности.
        /// </summary>
        /// <param name="executeAction">Действие, которое выполняет команда.</param>
        /// <param name="canExecutePredicate">Предикат доступности.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если executeAction равен null.</exception>
        public RelayCommand(Action<object?> executeAction, Predicate<object?>? canExecutePredicate = null)
        {
            _executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            _canExecutePredicate = canExecutePredicate;
        }

        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged;

        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            return _canExecutePredicate?.Invoke(parameter) ?? true;
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            _executeAction(parameter);
        }

        /// <summary>
        /// Сообщает UI, что нужно пересчитать возможность выполнения команды.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
