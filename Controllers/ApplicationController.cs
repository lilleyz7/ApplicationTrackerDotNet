using ApplicationTracker.Dtos;
using ApplicationTracker.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApplicationTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepo _repo;

        public ApplicationController(IApplicationRepo repo)
        {
            _repo = repo;
        }

        [Authorize]
        [HttpGet("/apps")]
        public IActionResult GetApps()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var applications = _repo.GetApplications(userId);
                if (applications == null)
                {
                    return Ok();
                }

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
        [HttpDelete("/delete")]
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
    }
}
