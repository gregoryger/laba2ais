using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.EF
{
    /// <summary>
    /// Контекст базы данных для работы с Entity Framework.
    /// </summary>
    public class GameDbContext : DbContext
    {
        /// <summary>
        /// Коллекция игр в базе данных.
        /// </summary>
        public DbSet<Game> Games { get; set; }

        /// <summary>
        /// Конструктор контекста базы данных.
        /// </summary>
        /// <param name="options">Параметры конфигурации контекста.</param>
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Конфигурация модели данных.
        /// </summary>
        /// <param name="modelBuilder">Построитель модели.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Genre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Rating).IsRequired();
            });
        }
    }
}
