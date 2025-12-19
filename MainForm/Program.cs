using System;
using System.Windows.Forms;
using GameApp.Presenter;
using Logic;

namespace GameApp.UI
{
    /// <summary>
    /// Точка входа WinForms-варианта приложения.
    /// Создает DI, выбирает провайдер репозитория и запускает MainForm.
    /// </summary>
    internal static class Program
    {
        private const string ConnectionString = "Server=LAPTOP-11O4LT8E\\SQLEXPRESS02;Database=GamesDB;Trusted_Connection=True;TrustServerCertificate=True;";

        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            var provider = AskProvider();
            var (view, scope) = AppStarter.Build(() => new MainForm(), ConnectionString, provider);
            using (scope)
            {
                Application.Run((Form)view);
            }
        }

        private static RepositoryProvider AskProvider()
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
