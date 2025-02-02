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
        private readonly ApplicationRepo _repo;

        public ApplicationController(ApplicationRepo repo)
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
                    return NotFound();
                }

                return Ok(applications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/addApp")]
        public async Task<IActionResult> AddApplication(ApplicationDto appToAdd)
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
                return BadRequest(ex);
            }
        }
    }
}
