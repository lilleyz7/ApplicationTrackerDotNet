using Microsoft.EntityFrameworkCore;
using ApplicationTracker.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApplicationTracker.Data
{
    public class ApplicationContext: IdentityDbContext<CustomUser>
    {
        public DbSet<Application> Applications { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: Apply configurations or constraints
            modelBuilder.Entity<CustomUser>()
            .HasMany(u => u.Applications)
            .WithOne()
            .HasForeignKey("UserId");

            modelBuilder.Entity<CustomUser>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

    }
}
