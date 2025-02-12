using ApplicationTracker.Entities;
using ApplicationTracker.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Repos
{
    public interface IApplicationRepo
    {
        ICollection<Application> GetApplications(string userId);
        ICollection<Application> GetByTitleAsync(string title, string userId);
        Task<Application> GetByIdAsync(Guid id, string userId);

        Task AddApplication(ApplicationDto application, string userId);
        Task UpdateApplication(ApplicationDto application, string userId, Guid appId);
        Task DeleteApplication(Guid id, string userId);
    }
}
