using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Models;

namespace DataAccessLayer.Dapper
{
    /// <summary>
    /// Репозиторий на базе Dapper.
    /// </summary>
    /// <typeparam name="T">Тип доменной сущности.</typeparam>
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly IDbConnection _connection;
        private readonly string _tableName;

        /// <summary>
        /// Создает репозиторий Dapper.
        /// </summary>
        /// <param name="connection">Открытое подключение.</param>
        /// <param name="tableName">Имя таблицы.</param>
        public DapperRepository(IDbConnection connection, string tableName)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _tableName = string.IsNullOrWhiteSpace(tableName) ? throw new ArgumentNullException(nameof(tableName)) : tableName;
        }

        /// <inheritdoc/>
        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != nameof(IDomainObject.Id))
                .Select(p => p.Name);

            var columns = string.Join(", ", properties);
            var values = string.Join(", ", properties.Select(p => "@" + p));
            var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({values}); SELECT CAST(SCOPE_IDENTITY() as int);";

            entity.Id = _connection.ExecuteScalar<int>(sql, entity);
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            _connection.Execute($"DELETE FROM {_tableName} WHERE Id = @Id", new { Id = id });
        }

        /// <inheritdoc/>
        public List<T> ReadAll()
        {
            return _connection.Query<T>($"SELECT * FROM {_tableName}").ToList();
        }

        /// <inheritdoc/>
        public T? ReadById(int id)
        {
            return _connection.QueryFirstOrDefault<T>($"SELECT * FROM {_tableName} WHERE Id = @Id", new { Id = id });
        }

        /// <inheritdoc/>
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var setClause = string.Join(", ",
                typeof(T).GetProperties()
                    .Where(p => p.Name != nameof(IDomainObject.Id))
                    .Select(p => $"{p.Name} = @{p.Name}"));

            _connection.Execute($"UPDATE {_tableName} SET {setClause} WHERE Id = @Id", entity);
        }
    }
}
