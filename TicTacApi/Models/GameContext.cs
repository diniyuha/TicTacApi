using Microsoft.EntityFrameworkCore;

namespace TicTacApi.Models
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options)
            : base(options)
        {
        }

        public DbSet<GameItem> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameItem>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<GameItem>().Property(x => x.Field).HasMaxLength(9);
        }
    }
}