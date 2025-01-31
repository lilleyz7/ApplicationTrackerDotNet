using Microsoft.EntityFrameworkCore;
using ApplicationTracker.Entities;

namespace ApplicationTracker.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<CustomUser> Users { get; set; }
        public DbSet<Application> Applications { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: Apply configurations or constraints
            modelBuilder.Entity<CustomUser>()
                .HasMany(c => c.Applications)
                .WithOne(c => c.CustomUser)
                .HasForeignKey(c => c.UserId)
                .HasPrincipalKey(c => c.Id);

            modelBuilder.Entity<CustomUser>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

    }
}
