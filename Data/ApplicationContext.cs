using Microsoft.EntityFrameworkCore;
using ApplicationTracker.Entities;

namespace ApplicationTracker.Data
{
    public class ApplicationContext: DbContext
    {
        DbSet<CustomUser> Users { get; set; }
        DbSet<Application> Applications { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }


    }
}
