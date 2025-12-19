using System;
using GameApp.Presenter.ViewModels;

namespace GameApp.Presenter.Navigation
{
    /// <summary>
    /// Создает ViewModel и уведомляет подписчиков через события.
    /// </summary>
    public sealed class ViewModelManager
    {
        private Func<MainViewModel>? _mainFactory;

        /// <summary>
        /// Событие: создана главная ViewModel.
        /// </summary>
        public event Action<MainViewModel>? MainViewModelCreated;

        /// <summary>
        /// Регистрирует фабрику создания главной ViewModel.
        /// </summary>
        /// <param name="factory">Фабрика MainViewModel.</param>
        public void RegisterMain(Func<MainViewModel> factory)
        {
            _mainFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Создает главную ViewModel и поднимает событие.
        /// </summary>
        public void RunMain()
        {
            if (_mainFactory == null)
            {
                throw new InvalidOperationException("Фабрика MainViewModel не зарегистрирована.");
            }

            var viewModel = _mainFactory();
            MainViewModelCreated?.Invoke(viewModel);
        }
    }
}
