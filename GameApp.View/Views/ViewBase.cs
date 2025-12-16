using System;
using System.Windows;
using GameApp.Presenter.ViewModels;

namespace GameApp.View.Views
{
    /// <summary>
    /// Базовый WPF-вид: только назначение DataContext.
    /// </summary>
    public class ViewBase : Window
    {
        /// <summary>
        /// Устанавливает DataContext переданной ViewModel.
        /// </summary>
        /// <param name="viewModel">Экземпляр ViewModel.</param>
        public void SetDataContext(ViewModelBase viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            DataContext = viewModel;
        }
    }
}
