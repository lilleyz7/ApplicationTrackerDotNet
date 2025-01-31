using ApplicationTracker.Data;
using ApplicationTracker.Entities;
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
        public ICollection<Application> GetApplicationsAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException("userId");
                }
                var applications = _db.Applications.Where(a => a.UserId == userId).OrderBy(a => a.DateAdded).ToList();
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

        public ICollection<Application> GetByTitleAsync(string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    throw new ArgumentNullException("title");
                }

                var applications = _db.Applications.Where(a => a.Title == title);
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
        public async Task<Application> GetByIdAsync(Guid id)
        {
            try
            {
                var application = await _db.Applications.FirstOrDefaultAsync(a => a.Id == id);
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
        public async Task AddApplication(Application application, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId)) { throw new ArgumentNullException("userId"); }
                var user = _db.Users.Include(u => u.Applications).FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("User does not exist");
                }

                application.UserId = userId;
                application.CustomUser = user;

                // Add to user's collection
                user.Applications.Add(application);

                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"failed to add application with error: {ex}");
            }
        }

        public async void DeleteApplication(Guid id, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId)) { throw new ArgumentNullException("userId"); }
                var user = _db.Users.Include(u => u.Applications).FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    throw new Exception("User does not exist");
                }

                var application = user.Applications.FirstOrDefault(a => a.Id == id);
                if (application == null)
                {
                    return;
                }
                user.Applications.Remove(application);

                int success = await SaveChangesAsync();


            }
            catch (Exception ex)
            {
                throw new Exception($"Failed with error: {ex}");
            }
        }


        public void UpdateApplication(Application updatedApplication, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
