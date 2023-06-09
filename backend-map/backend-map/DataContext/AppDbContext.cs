using backend_map.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_map.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(
                aUser =>
                {
                    aUser.HasKey(e => e.Id);
                    aUser.Property(e => e.PasswordHash)
                    .HasColumnType("varbinary(max)");
                    aUser.Property(e => e.PasswordSalt)
                    .HasColumnType("varbinary(max)");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
