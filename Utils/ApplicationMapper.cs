using ApplicationTracker.Dtos;
using ApplicationTracker.Entities;

namespace ApplicationTracker.Utils
{
    public class ApplicationMapper
    {
        public static Application Map(ApplicationDto applicationDto, CustomUser user)
        {
            return new Application
            {
                Company = applicationDto.Company,
                Title = applicationDto.Title,
                Notes = applicationDto.Notes,
                Status = applicationDto.Status,
                Link = applicationDto.Link,
                UserId = user.Id,
                CustomUser = user,
            };
        }
    }
}
