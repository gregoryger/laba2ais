using System;
using Logic;
using Ninject;

namespace ConsoleApp
{
    /// <summary>
    /// Точка входа консольного приложения с выбором провайдера и инициализацией Ninject.
    /// </summary>
    internal static class Program
    {
        private const string ConnectionString = "Server=LAPTOP-11O4LT8E\\SQLEXPRESS02;Database=GamesDB;Trusted_Connection=True;TrustServerCertificate=True;";

        private static void Main()
        {
            var provider = AskForProvider();

            using var kernel = new StandardKernel(new SimpleConfigModule(ConnectionString, provider));
            var logic = kernel.Get<IGameLogic>();

            ConsoleMethods.Run(logic);
        }

        private static RepositoryProvider AskForProvider()
        {
            while (true)
            {
                Console.WriteLine("Выберите провайдера данных:");
                Console.WriteLine("1 - Entity Framework Core");
                Console.WriteLine("2 - Dapper");
                Console.Write("Введите номер: ");
                var input = Console.ReadLine();

                if (input == "1")
                {
                    return RepositoryProvider.EntityFramework;
                }

                if (input == "2")
                {
                    return RepositoryProvider.Dapper;
                }

                Console.WriteLine("Некорректный ввод. Введите 1 или 2.\n");
            }
        }
    }
}
