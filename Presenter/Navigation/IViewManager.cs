using GameApp.Presenter.ViewModels;

namespace GameApp.Presenter.Navigation
{
    /// <summary>
    /// Показывает View для указанной ViewModel.
    /// </summary>
    public interface IViewManager
    {
        /// <summary>
        /// Показывает View, зарегистрированный для указанной ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">Тип ViewModel.</typeparam>
        /// <param name="viewModel">Экземпляр ViewModel.</param>
        void Show<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase;
    }
}
