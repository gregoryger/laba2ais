using System;
using System.Collections.Generic;
using GameApp.Presenter.ViewModels;

namespace GameApp.Presenter.Navigation
{
    /// <summary>
    /// Простой менеджер ViewModel, который хранит фабрики по именам типов.
    /// </summary>
    public class ViewModelManager : IViewModelManager
    {
        private readonly Dictionary<string, Func<ViewModelBase>> _factories = new();

        /// <inheritdoc />
        public void Register<TViewModel>(Func<TViewModel> factory)
            where TViewModel : ViewModelBase
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var key = typeof(TViewModel).FullName ?? typeof(TViewModel).Name;
            _factories[key] = () => factory();
        }

        /// <inheritdoc />
        public TViewModel Get<TViewModel>()
            where TViewModel : ViewModelBase
        {
            var key = typeof(TViewModel).FullName ?? typeof(TViewModel).Name;
            if (!_factories.TryGetValue(key, out var factory))
            {
                throw new InvalidOperationException($"ViewModel для {key} не зарегистрирована.");
            }

            return (TViewModel)factory();
        }
    }
}
