using System.Collections.Generic;
using Models;

namespace DataAccessLayer
{
    /// <summary>
    /// Универсальный интерфейс CRUD для доменных сущностей.
    /// </summary>
    /// <typeparam name="T">Тип доменной сущности.</typeparam>
    public interface IRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Создает сущность.
        /// </summary>
        /// <param name="entity">Экземпляр сущности.</param>
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
        /// <param name="id">Идентификатор.</param>
        /// <returns>Экземпляр или null.</returns>
        T? ReadById(int id);

        /// <summary>
        /// Обновляет данные сущности.
        /// </summary>
        /// <param name="entity">Сущность с изменениями.</param>
        void Update(T entity);
    }
}
