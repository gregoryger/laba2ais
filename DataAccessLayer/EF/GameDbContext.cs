using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.EF
{
    /// <summary>
    /// Контекст EF Core для работы с таблицей игр.
    /// </summary>
    public class GameDbContext : DbContext
    {
        /// <summary>
        /// Создает контекст.
        /// </summary>
        /// <param name="options">Параметры контекста.</param>
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Набор игр.
        /// </summary>
        public DbSet<Game> Games { get; set; } = null!;

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
