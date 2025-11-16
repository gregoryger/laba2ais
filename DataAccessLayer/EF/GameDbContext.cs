using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.EF
{
    /// <summary>
    /// DbContext Entity Framework Core для каталога игр.
    /// </summary>
    public class GameDbContext : DbContext
    {
        /// <summary>
        /// Инициализирует контекст переданными опциями.
        /// </summary>
        /// <param name="options">Параметры EF Core.</param>
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
