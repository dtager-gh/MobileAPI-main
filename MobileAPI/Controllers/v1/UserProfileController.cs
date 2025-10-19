using Logto.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Logto.AspNetCore.Authentication.LogtoParameters;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/User/UserProfile")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileService _userProfileService;
        private readonly ILogger<UserProfileController> _logger;
        
        public UserProfileController(UserProfileService userProfileService, ILogger<UserProfileController> logger)
        {
            _userProfileService = userProfileService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all user profiles.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("UserProfile GetAll called at {Time}", DateTime.UtcNow);

            String? userId = User.Claims.FirstOrDefault(c => c.Type == LogtoParameters.Claims.Subject)?.Value; ;

            if (userId == null)
            {
                return Unauthorized("You must be logged in to retrieve user preferences");
            }

            return Ok(User.Claims.ToList());
        }

        /// <summary>
        /// Gets a user profile by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            String userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest();
            }

            var userProfile = await _userProfileService.FindAsync(id, userId);

            if (userProfile == null)
            {
                return NotFound();
            }
            
            return Ok(userProfile);
        }

        /// <summary>
        /// Creates a user profile.
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return BadRequest("Invalid UserProfile data.");
            }
            
            await _userProfileService.AddAsync(userProfile);
            
            return CreatedAtAction(nameof(GetById), new { id = userProfile.Id }, userProfile);
        }

        /// <summary>
        /// Updates a user profile.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UserProfile userProfile)
        {
            if (userProfile == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid UserProfile data.");
            }
            
            var existingUserProfile= await _userProfileService.FindAsync(userProfile.Id);
            if (existingUserProfile == null)
            {
                return NotFound();
            }

            if (existingUserProfile.Timestamp != userProfile.Timestamp) 
            {
                return BadRequest("Concurrency check failed");
            }
            
            await _userProfileService.UpdateAsync(userProfile);
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a user profile.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingUserProfile= await _userProfileService.FindAsync(id);
            if (existingUserProfile == null)
            {
                return NotFound();
            }

            if (existingUserProfile.Timestamp != timestamp) 
            {
                return BadRequest("Concurrency check failed.");
            }

            await _userProfileService.DeleteAsync(existingUserProfile);
            
            return NoContent();
        }
    }
}