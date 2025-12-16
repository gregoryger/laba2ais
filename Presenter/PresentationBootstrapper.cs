using System;
using GameApp.Presenter.Services;
using GameApp.Presenter.ViewModels;
using Logic;
using Ninject;

namespace GameApp.Presenter
{
    /// <summary>
    /// Создает и хранит композиционный корень для слоя Presenter.
    /// </summary>
    public sealed class PresentationBootstrapper : IDisposable
    {
        private readonly IKernel _kernel;
        private bool _disposed;

        /// <summary>
        /// Инициализирует композиционный корень с настройками базы данных.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе.</param>
        /// <param name="provider">Выбранный провайдер репозитория.</param>
        public PresentationBootstrapper(string connectionString, RepositoryProvider provider)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string must be provided.", nameof(connectionString));
            }

            _kernel = new StandardKernel(new SimpleConfigModule(connectionString, provider));
        }

        /// <summary>
        /// Инициализирует композиционный корень c Entity Framework Core по умолчанию.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе.</param>
        public PresentationBootstrapper(string connectionString)
            : this(connectionString, RepositoryProvider.EntityFramework)
        {
        }

        /// <summary>
        /// Создает главную ViewModel со всеми необходимыми сервисами.
        /// </summary>
        /// <param name="userInteractionService">Сервис взаимодействия с пользователем.</param>
        /// <param name="fileDialogService">Сервис выбора файлов.</param>
        /// <param name="viewManager">Менеджер View.</param>
        /// <returns>Подготовленная MainViewModel.</returns>
        public MainViewModel CreateMainViewModel(
            IUserInteractionService userInteractionService,
            IFileDialogService fileDialogService,
            Navigation.IViewManager viewManager)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(PresentationBootstrapper));
            }

            var logic = _kernel.Get<IGameLogic>();
            return new MainViewModel(logic, userInteractionService, fileDialogService, viewManager);
        }

        /// <summary>
        /// Освобождает созданный контейнер зависимостей.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _kernel.Dispose();
            _disposed = true;
        }
    }
}
