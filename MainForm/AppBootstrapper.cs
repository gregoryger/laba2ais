using System;
using System.Windows.Forms;
using GameApp.Shared;
using Logic;
using Ninject;

namespace GameApp.UI
{
    /// <summary>
    /// Сборка зависимостей и создание главной формы.
    /// </summary>
    public static class AppBootstrapper
    {
        /// <summary>
        /// Собирает ядро DI, создаёт View и Presenter.
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД.</param>
        /// <param name="provider">Выбранный провайдер данных.</param>
        /// <returns>Экземпляр главной формы.</returns>
        public static Form BuildMainForm(string connectionString, RepositoryProvider provider)
        {
            var kernel = CreateKernel(connectionString, provider);
            var logic = kernel.Get<IGameLogic>();

            var view = new MainForm();
            var presenter = new GameApp.Presenter.MainPresenter(view, logic);
            view.Disposed += (_, _) => kernel.Dispose();
            return view;
        }

        private static IKernel CreateKernel(string connectionString, RepositoryProvider provider)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Строка подключения не должна быть пустой.", nameof(connectionString));
            }

            return new StandardKernel(new SimpleConfigModule(connectionString, provider));
        }
    }
}
