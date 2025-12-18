using System;
using GameApp.Presenter.ViewModels;

namespace GameApp.Presenter.Navigation
{
    /// <summary>
    /// Управляет регистрацией и получением ViewModel.
    /// </summary>
    public interface IViewModelManager
    {
        /// <summary>
        /// Регистрирует фабрику для указанной ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">Тип ViewModel.</typeparam>
        /// <param name="factory">Фабрика создания экземпляров.</param>
        void Register<TViewModel>(Func<TViewModel> factory)
            where TViewModel : ViewModelBase;

        /// <summary>
        /// Возвращает новую ViewModel, созданную из зарегистрированной фабрики.
        /// </summary>
        /// <typeparam name="TViewModel">Тип ViewModel.</typeparam>
        /// <returns>Экземпляр ViewModel.</returns>
        TViewModel Get<TViewModel>()
            where TViewModel : ViewModelBase;
    }
}
