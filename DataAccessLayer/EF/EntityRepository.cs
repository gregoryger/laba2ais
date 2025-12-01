using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.EF
{
    /// <summary>
    /// Репозиторий на базе Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">Тип доменной сущности.</typeparam>
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly GameDbContext _context;
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Создает репозиторий.
        /// </summary>
        /// <param name="context">Контекст EF Core.</param>
        public EntityRepository(GameDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        /// <inheritdoc/>
        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        /// <inheritdoc/>
        public List<T> ReadAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        /// <inheritdoc/>
        public T? ReadById(int id)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(e => e.Id == id);
        }

        /// <inheritdoc/>
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
