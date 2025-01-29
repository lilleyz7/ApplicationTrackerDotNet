using Microsoft.EntityFrameworkCore;

namespace ApplicationTracker.Data
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
    }
}
