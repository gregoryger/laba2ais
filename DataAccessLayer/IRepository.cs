using System.Collections.Generic;
using Models;

namespace DataAccessLayer
{
    /// <summary>
    /// Интерфейс репозитория для выполнения CRUD-операций над доменными объектами.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта, реализующего IDomainObject.</typeparam>
    public interface IRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Добавляет новый объект в хранилище.
        /// </summary>
        /// <param name="entity">Объект для добавления.</param>
        void Add(T entity);

        /// <summary>
        /// Удаляет объект из хранилища по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта для удаления.</param>
        void Delete(int id);

        /// <summary>
        /// Возвращает все объекты из хранилища.
        /// </summary>
        /// <returns>Список всех объектов.</returns>
        List<T> ReadAll();

        /// <summary>
        /// Возвращает объект по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомого объекта.</param>
        /// <returns>Объект с указанным идентификатором или null.</returns>
        T ReadById(int id);

        /// <summary>
        /// Обновляет существующий объект в хранилище.
        /// </summary>
        /// <param name="entity">Объект с обновленными данными.</param>
        void Update(T entity);
    }
}
