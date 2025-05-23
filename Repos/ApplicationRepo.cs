using ApplicationTracker.Data;
using ApplicationTracker.Dtos;
using ApplicationTracker.Entities;
using ApplicationTracker.Utils;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTracker.Repos
{
    public class ApplicationRepo : IApplicationRepo
    {
        public readonly ApplicationContext _db;

        public ApplicationRepo(ApplicationContext context)
        {
            _db = context;
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
        public ICollection<Application> GetApplications(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException("userId");
                }

                var applications = _db.Applications.Where(a => a.UserId == userId).ToList();
                //var user = _db.Users.Include(u => u.Applications).FirstOrDefault(u => u.Id == userId);
                //if (user == null)
                //{
                //    throw new Exception("Failed to find user");
                //}
                //var applications = user.Applications.OrderBy(a => a.DateAdded).ToList();
                if (applications == null)
                {
                    return new List<Application>();
                }

                return applications;
            }
            catch (Exception ex)
            {
                throw new Exception($"failed to get applications with error: {ex}");
            }
        }

        public ICollection<Application> GetByTitleAsync(string title, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    throw new ArgumentNullException("title");
                }

                var user = _db.Users.Include(u => u.Applications).FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var applications = user.Applications.Where(a => a.Title == title);
                if (applications == null)
                {
                    return new List<Application>();
                }
                return applications.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"failed to get application with error: {ex}");
            }
        }
        public async Task<Application> GetByIdAsync(Guid id, string userId)
        {
            try
            {
                var user = await _db.Users.Include(u => u.Applications).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null) {
                    throw new Exception("User not found");
                }
                var application = user.Applications.Where(a => a.Id == id).FirstOrDefault();
                if (application == null)
                {
                    throw new Exception("Application does not exist");
                }
                return application;
            }
            catch (Exception ex)
            {
                throw new Exception($"failed to get application with error: {ex}");
            }
        }
        public async Task AddApplication(ApplicationDto application, string userId)
        {
            try
            {

                if (application == null)
                {
                    throw new ArgumentNullException("Application");
                }

                if (string.IsNullOrEmpty(userId)) { throw new ArgumentNullException("userId"); }

                var user = _db.Users.Include(u => u.Applications).FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("User does not exist");
                }

                var applicationToAdd = ApplicationMapper.Map(application, user);

                // Add to user's collection
                _db.Applications.Add(applicationToAdd);

                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"failed to add application with error: {ex}");
            }
        }

        public async Task DeleteApplication(Guid id, string userId)
        {
            try
            {
                var applicationToDelete = _db.Applications.FirstOrDefault(a => a.Id == id);
                if (applicationToDelete == null)
                {
                    throw new Exception("Application does not exist");
                }

                _db.Applications.Remove(applicationToDelete);
  
                int changesMade = await SaveChangesAsync();
                if (changesMade <= 0)
                {
                    throw new Exception("Failed to delete application");
                }


            }
            catch (Exception ex)
            {
                throw new Exception($"Failed with error: {ex}");
            }
        }
        public async Task UpdateApplication(ApplicationDto updatedApplication, string userId, Guid appId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException("userId");
                }

                Application application = await _db.Applications.FirstAsync(a => a.Id == appId);
                if (application == null)
                {
                    throw new ArgumentException("appId");
                }

                application.Company = updatedApplication.Company;
                application.Status = updatedApplication.Status;
                application.Notes = updatedApplication.Notes;
                application.Title = updatedApplication.Title;
                application.Link = updatedApplication.Link;

                int changes = await SaveChangesAsync();
                if (changes <= 0)
                {
                    throw new Exception("Failed to update");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update with error: {ex}");
            }
        }
    }
}
