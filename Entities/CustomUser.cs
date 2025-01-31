using Microsoft.AspNetCore.Identity;

namespace ApplicationTracker.Entities
{
    public class CustomUser: IdentityUser
    {
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
