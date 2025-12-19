using System;
using System.Collections.Generic;
using System.Windows;
using GameApp.Presenter.Navigation;
using GameApp.Presenter.ViewModels;

namespace GameApp.View.Views
{
    /// <summary>
    /// Показывает WPF окна для созданных ViewModel (без строковых ключей).
    /// </summary>
    public sealed class ViewManager
    {
        private readonly Dictionary<ViewModelBase, ViewBase> _openedViews =
            new Dictionary<ViewModelBase, ViewBase>();

        /// <summary>
        /// Подписывается на события создания ViewModel.
        /// </summary>
        /// <param name="viewModelManager">Менеджер ViewModel.</param>
        public ViewManager(ViewModelManager viewModelManager)
        {
            if (viewModelManager == null)
            {
                throw new ArgumentNullException(nameof(viewModelManager));
            }

            viewModelManager.MainViewModelCreated += OnMainViewModelCreated;
        }

        private void OnMainViewModelCreated(MainViewModel viewModel)
        {
            if (_openedViews.TryGetValue(viewModel, out var existing))
            {
                existing.Activate();
                return;
            }

            var view = new MainView();
            view.SetDataContext(viewModel);

            _openedViews[viewModel] = view;

            if (Application.Current.MainWindow == null)
            {
                Application.Current.MainWindow = view;
            }

            view.Show();
        }
    }
}
