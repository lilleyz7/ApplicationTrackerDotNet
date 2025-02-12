using ApplicationTracker.Dtos;
using ApplicationTracker.Entities;
using ApplicationTracker.Redis;
using ApplicationTracker.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ApplicationTracker.Controllers
{
    [EnableRateLimiting("fixed")]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepo _repo;
        //private readonly IRedisService _cache;

        public ApplicationController(IApplicationRepo repo /*IRedisService redisService*/ )
        {
            _repo = repo;
            //_cache = redisService;
        }

        [Authorize]
        [HttpGet("/apps")]
        public IActionResult GetAppsByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            //var cachedData = _cache.GetData<List<Application>>($"apps/{userId}");
            //if (cachedData is not null)
            //{
            //    return Ok(cachedData);
            //}

            try
            {
                var applications = _repo.GetApplications(userId);
                if (applications == null)
                {
                    return Ok();
                }

                //_cache.SetData<List<Application>>($"apps/{userId}", applications.ToList());
                return Ok(applications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("/addApp")]
        public async Task<IActionResult> AddApplication([FromBody] ApplicationDto appToAdd)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (appToAdd == null)
            {
                return BadRequest("invalid body");
            }

            try
            {
                await _repo.AddApplication(appToAdd, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("/delete/{appId}")]
        public async Task<IActionResult> DeleteApplication(string appId)
        {
            if (string.IsNullOrEmpty(appId))
            {
                return BadRequest("Invalid Application");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                Guid guidId = Guid.Parse(appId);
                await _repo.DeleteApplication(guidId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize]
        [HttpPut("/update/{appId}")]
        public async Task<IActionResult> UpdateApplication([FromBody] ApplicationDto appToUpdate, string appId)
        {
            if (appToUpdate == null)
            {
                return BadRequest("Invalid Application");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                Guid guidId = Guid.Parse(appId);
                await _repo.UpdateApplication(appToUpdate, userId, guidId);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed with error {ex.Message}");
            }
        }
    }
}
