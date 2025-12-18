using System;
using System.Collections.Generic;
using System.Windows;
using GameApp.Presenter.Navigation;
using GameApp.Presenter.ViewModels;

namespace GameApp.View.Views
{
    /// <summary>
    /// Создает и отображает WPF-окна для заданных ViewModel.
    /// </summary>
    public class ViewManager : IViewManager
    {
        private readonly Dictionary<string, Func<ViewBase>> _viewFactories = new();

        /// <summary>
        /// Регистрирует соответствие между типами ViewModel и View.
        /// </summary>
        /// <typeparam name="TViewModel">Тип ViewModel.</typeparam>
        /// <typeparam name="TView">Тип View, наследуется от ViewBase.</typeparam>
        public void Register<TViewModel, TView>()
            where TViewModel : ViewModelBase
            where TView : ViewBase, new()
        {
            var key = typeof(TViewModel).FullName ?? typeof(TViewModel).Name;
            _viewFactories[key] = () => new TView();
        }

        /// <summary>
        /// Показывает View для указанной ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">Тип ViewModel.</typeparam>
        /// <param name="viewModel">Экземпляр ViewModel.</param>
        public void Show<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var key = typeof(TViewModel).FullName ?? typeof(TViewModel).Name;
            if (!_viewFactories.TryGetValue(key, out var viewFactory))
            {
                throw new InvalidOperationException($"View для {key} не зарегистрирован.");
            }

            var view = viewFactory();
            view.SetDataContext(viewModel);

            if (Application.Current.MainWindow == null)
            {
                Application.Current.MainWindow = view;
            }

            view.Show();
        }
    }
}
