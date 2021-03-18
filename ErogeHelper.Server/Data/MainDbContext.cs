using ErogeHelper.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace ErogeHelper.Server.Data
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasMany(it => it.Names)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Game> Games => Set<Game>();
        public DbSet<User> User => Set<User>();
        public DbSet<Subtitle> Subtitles => Set<Subtitle>();
    }
}