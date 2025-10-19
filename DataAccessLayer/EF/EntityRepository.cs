using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.EF
{
    /// <summary>
    /// Реализация репозитория с использованием Entity Framework.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly GameDbContext _context;
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Конструктор репозитория.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public EntityRepository(GameDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Добавляет новый объект в базу данных.
        /// </summary>
        /// <param name="entity">Объект для добавления.</param>
        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Удаляет объект из базы данных по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Возвращает все объекты из базы данных.
        /// </summary>
        /// <returns>Список всех объектов.</returns>
        public List<T> ReadAll()
        {
            return _dbSet.ToList();
        }

        /// <summary>
        /// Возвращает объект по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns>Объект или null.</returns>
        public T ReadById(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Обновляет существующий объект в базе данных.
        /// </summary>
        /// <param name="entity">Объект с обновленными данными.</param>
        public void Update(T entity)
        {
            var tracked = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);

            if (tracked != null)
            {
                // Отсоединяем старую сущность
                _context.Entry(tracked).State = EntityState.Detached;
            }

            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

    }
}
