using System;
using System.Windows.Forms;
using Logic;
using Ninject;

namespace GameApp.UI
{
    /// <summary>
    /// Точка входа WinForms с настройкой зависимостей через Ninject.
    /// </summary>
    internal static class Program
    {
        private const string ConnectionString = "Server=LAPTOP-11O4LT8E\\SQLEXPRESS02;Database=GamesDB;Trusted_Connection=True;TrustServerCertificate=True;";

        /// <summary>
        /// Точка входа приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            var provider = AskRepositoryProvider();

            using var kernel = new StandardKernel(new SimpleConfigModule(ConnectionString, provider));
            var logic = kernel.Get<IGameLogic>();

            Application.Run(new MainForm(logic));
        }

        private static RepositoryProvider AskRepositoryProvider()
        {
            var result = MessageBox.Show(
                "Использовать Entity Framework Core для работы с данными?\nВыберите \"Нет\", чтобы переключиться на Dapper.",
                "Выбор источника данных",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            return result == DialogResult.Yes
                ? RepositoryProvider.EntityFramework
                : RepositoryProvider.Dapper;
        }
    }
}
