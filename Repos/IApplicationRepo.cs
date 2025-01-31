using ApplicationTracker.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Repos
{
    public interface IApplicationRepo
    {
        ICollection<Application> GetApplicationsAsync(string userId);
        ICollection<Application> GetByTitleAsync(string title);
        Task<Application> GetByIdAsync(Guid id);

        IActionResult AddApplication(Application application, string userId);
        void UpdateApplication(Application application, string userId);
        void DeleteApplication(Guid id);
    }
}
