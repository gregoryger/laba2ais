using System.Windows;
using GameApp.Presenter;
using GameApp.Presenter.Navigation;
using GameApp.Presenter.ViewModels;
using GameApp.View.Services;
using GameApp.View.Views;

namespace GameApp.View
{
    /// <summary>
    /// Точка входа WPF-приложения: собирает Presenter и View.
    /// </summary>
    public partial class App : Application
    {
        private PresentationBootstrapper? _bootstrapper;
        private ViewManager? _viewManager;
        private ViewModelManager? _viewModelManager;

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            const string connectionString = "Server=LAPTOP-11O4LT8E\\SQLEXPRESS02;Database=GamesDB;Trusted_Connection=True;TrustServerCertificate=True;";

            _viewManager = new ViewManager();
            _viewManager.Register<MainViewModel, MainView>();
            _viewModelManager = new ViewModelManager();

            _bootstrapper = new PresentationBootstrapper(connectionString);
            var dialogService = new DialogService();
            var fileDialogService = new FileDialogService();

            _viewModelManager.Register(() => _bootstrapper.CreateMainViewModel(dialogService, fileDialogService, _viewManager));

            var mainViewModel = _viewModelManager.Get<MainViewModel>();
            mainViewModel.ShowView();
            mainViewModel.LoadData();
        }

        /// <inheritdoc />
        protected override void OnExit(ExitEventArgs e)
        {
            _bootstrapper?.Dispose();
            base.OnExit(e);
        }
    }
}
