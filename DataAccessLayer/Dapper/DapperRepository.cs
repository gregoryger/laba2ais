using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Models;

namespace DataAccessLayer.Dapper
{
    /// <summary>
    /// Реализация репозитория с использованием Dapper.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly IDbConnection _connection;
        private readonly string _tableName;

        /// <summary>
        /// Конструктор репозитория.
        /// </summary>
        /// <param name="connection">Соединение с базой данных.</param>
        /// <param name="tableName">Название таблицы в базе данных.</param>
        public DapperRepository(IDbConnection connection, string tableName)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        /// <summary>
        /// Добавляет новый объект в базу данных.
        /// </summary>
        /// <param name="entity">Объект для добавления.</param>
        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id")
                .Select(p => p.Name);

            var columns = string.Join(", ", properties);
            var values = string.Join(", ", properties.Select(p => "@" + p));

            var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({values}); SELECT CAST(SCOPE_IDENTITY() as int);";

            entity.Id = _connection.ExecuteScalar<int>(sql, entity);
        }

        /// <summary>
        /// Удаляет объект из базы данных по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        public void Delete(int id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
            _connection.Execute(sql, new { Id = id });
        }

        /// <summary>
        /// Возвращает все объекты из базы данных.
        /// </summary>
        /// <returns>Список всех объектов.</returns>
        public List<T> ReadAll()
        {
            var sql = $"SELECT * FROM {_tableName}";
            return _connection.Query<T>(sql).ToList();
        }

        /// <summary>
        /// Возвращает объект по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns>Объект или null.</returns>
        public T ReadById(int id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            return _connection.QueryFirstOrDefault<T>(sql, new { Id = id });
        }

        /// <summary>
        /// Обновляет существующий объект в базе данных.
        /// </summary>
        /// <param name="entity">Объект с обновленными данными.</param>
        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id")
                .Select(p => $"{p.Name} = @{p.Name}");

            var setClause = string.Join(", ", properties);
            var sql = $"UPDATE {_tableName} SET {setClause} WHERE Id = @Id";

            _connection.Execute(sql, entity);
        }
    }
}
