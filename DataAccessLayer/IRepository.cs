using System.Collections.Generic;
using Models;

namespace DataAccessLayer
{
    /// <summary>
    /// Абстракция репозитория с базовыми CRUD-операциями для доменных объектов.
    /// </summary>
    /// <typeparam name="T">Тип доменной сущности.</typeparam>
    public interface IRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Добавляет новую сущность в хранилище.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        void Add(T entity);

        /// <summary>
        /// Удаляет сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        void Delete(int id);

        /// <summary>
        /// Возвращает все сущности.
        /// </summary>
        /// <returns>Список сущностей.</returns>
        List<T> ReadAll();

        /// <summary>
        /// Возвращает сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор для поиска.</param>
        /// <returns>Сущность или null.</returns>
        T? ReadById(int id);

        /// <summary>
        /// Обновляет сущность в хранилище.
        /// </summary>
        /// <param name="entity">Сущность с обновлёнными полями.</param>
        void Update(T entity);
    }
}
