using System;
using System.Data;
using DataAccessLayer;
using DataAccessLayer.Dapper;
using DataAccessLayer.EF;
using Logic.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Ninject.Modules;
using Models;

namespace Logic
{
    /// <summary>
    /// Конфигурационный модуль Ninject, который связывает репозитории и вспомогательные сервисы.
    /// </summary>
    public class SimpleConfigModule : NinjectModule
    {
        private readonly string _connectionString;
        private readonly RepositoryProvider _provider;

        /// <summary>
        /// Создаёт модуль конфигурации с выбранным провайдером.
        /// </summary>
        /// <param name="connectionString">Строка подключения к SQL Server.</param>
        /// <param name="provider">Выбранный источник данных.</param>
        public SimpleConfigModule(string connectionString, RepositoryProvider provider)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string must not be empty.", nameof(connectionString));
            }

            _connectionString = connectionString;
            _provider = provider;
        }

        /// <inheritdoc/>
        public override void Load()
        {
            Bind<IGameLogger>()
                .To<FileGameLogger>()
                .InSingletonScope()
                .WithConstructorArgument("logFilePath", "logs/gameapp.log");

            Bind<IGameValidator>()
                .To<GameValidator>()
                .InSingletonScope();

            switch (_provider)
            {
                case RepositoryProvider.EntityFramework:
                    Bind<DbContextOptions<GameDbContext>>()
                        .ToMethod(_ => BuildOptions())
                        .InSingletonScope();

                    Bind<GameDbContext>()
                        .ToSelf()
                        .InSingletonScope()
                        .OnActivation(context => context.Database.EnsureCreated());

                    Bind<IRepository<Game>>()
                        .To<EntityRepository<Game>>()
                        .InSingletonScope();
                    break;

                case RepositoryProvider.Dapper:
                    EnsureDatabaseCreated();

                    Bind<IDbConnection>()
                        .ToMethod(_ =>
                        {
                            var connection = new SqlConnection(_connectionString);
                            connection.Open();
                            return connection;
                        })
                        .InSingletonScope()
                        .OnDeactivation(connection => connection.Dispose());

                    Bind<IRepository<Game>>()
                        .To<DapperRepository<Game>>()
                        .InSingletonScope()
                        .WithConstructorArgument("tableName", "Games");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(_provider), _provider, "Unsupported repository provider.");
            }

            Bind<IGameLogic>().To<GameLogic>().InSingletonScope();
        }

        private DbContextOptions<GameDbContext> BuildOptions()
        {
            return new DbContextOptionsBuilder<GameDbContext>()
                .UseSqlServer(_connectionString)
                .Options;
        }

        private void EnsureDatabaseCreated()
        {
            var options = BuildOptions();
            using var context = new GameDbContext(options);
            context.Database.EnsureCreated();
        }
    }
}
