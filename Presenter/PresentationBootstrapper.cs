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

        public PresentationBootstrapper(string connectionString, RepositoryProvider provider)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string must be provided.", nameof(connectionString));
            }

            _kernel = new StandardKernel(new SimpleConfigModule(connectionString, provider));
        }

        public PresentationBootstrapper(string connectionString)
            : this(connectionString, RepositoryProvider.EntityFramework)
        {
        }

        /// <summary>
        /// Создает главную ViewModel со всеми необходимыми сервисами.
        /// </summary>
        /// <param name="userInteractionService">Сервис взаимодействия с пользователем.</param>
        /// <param name="fileDialogService">Сервис выбора файлов.</param>
        /// <returns>Подготовленная MainViewModel.</returns>
        public MainViewModel CreateMainViewModel(
            IUserInteractionService userInteractionService,
            IFileDialogService fileDialogService)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(PresentationBootstrapper));
            }

            var logic = _kernel.Get<IGameLogic>();
            return new MainViewModel(logic, userInteractionService, fileDialogService);
        }

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
