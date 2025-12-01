using System;
using System.Windows.Forms;
using Logic;

namespace GameApp.UI
{
    /// <summary>
    /// Точка входа WinForms-приложения.
    /// </summary>
    internal static class Program
    {
        private const string ConnectionString = "Server=LAPTOP-11O4LT8E\\SQLEXPRESS02;Database=GamesDB;Trusted_Connection=True;TrustServerCertificate=True;";

        /// <summary>
        /// Запуск приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            var provider = AskRepositoryProvider();
            var form = AppBootstrapper.BuildMainForm(ConnectionString, provider);
            Application.Run(form);
        }

        private static RepositoryProvider AskRepositoryProvider()
        {
            var result = MessageBox.Show(
                "Использовать Entity Framework Core? (Нет = Dapper)",
                "Выбор провайдера данных",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            return result == DialogResult.Yes
                ? RepositoryProvider.EntityFramework
                : RepositoryProvider.Dapper;
        }
    }
}
