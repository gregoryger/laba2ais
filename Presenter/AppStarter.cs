using System;
using GameApp.Shared;
using Logic;
using Ninject;

namespace GameApp.Presenter
{
    /// <summary>
    /// Стартовый построитель приложения: создает DI-ядро, View и Presenter.
    /// </summary>
    public static class AppStarter
    {
        /// <summary>
        /// Создает View и Presenter, возвращает View, Scope для запуска UI.
        /// </summary>
        /// <param name="viewFactory">Представление.</param>
        /// <param name="connectionString">Строка подключения к БД.</param>
        /// <param name="provider">Выбранный провайдер данных.</param>
        /// <returns>Кортеж с View и IDisposable-объектом (ядро DI).</returns>
        public static (IMainView View, IDisposable Scope) Build(Func<IMainView> viewFactory, string connectionString, RepositoryProvider provider)
        {
            if (viewFactory == null)
            {
                throw new ArgumentNullException(nameof(viewFactory));
            }

            var kernel = new StandardKernel(new SimpleConfigModule(connectionString, provider));
            var logic = kernel.Get<IGameLogic>();
            var view = viewFactory();
            var presenter = new MainPresenter(view, logic);

            // presenter подписывается в конструкторе, дополнительных вызовов не нужно
            return (view, kernel);
        }
    }
}
