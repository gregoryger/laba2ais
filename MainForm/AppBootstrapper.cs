using System;
using System.Windows.Forms;
using GameApp.Presenter;
using GameApp.Shared;
using Logic;
using Ninject;

namespace GameApp.UI
{
    /// <summary>
    /// Точка сборки зависимостей и главного окна.
    /// </summary>
    public static class AppBootstrapper
    {
        /// <summary>
        /// Строит главное окно приложения.
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД.</param>
        /// <param name="provider">Провайдер данных.</param>
        /// <returns>Настроенная форма.</returns>
        public static Form BuildMainForm(string connectionString, RepositoryProvider provider)
        {
            var kernel = new StandardKernel(new SimpleConfigModule(connectionString, provider));
            var logic = kernel.Get<IGameLogic>();
            IMainView view = new MainForm();
            var presenter = new MainPresenter(view, logic);
            presenter.Initialize();

            if (view is Form form)
            {
                form.Disposed += (_, _) => kernel.Dispose();
                return form;
            }

            kernel.Dispose();
            throw new InvalidOperationException("View должна быть формой WinForms.");
        }
    }
}
