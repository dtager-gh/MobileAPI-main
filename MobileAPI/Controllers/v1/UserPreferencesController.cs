using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/User/UserPreference")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserPreferencesController : ControllerBase
    {
        private readonly UserPreferencesService _userPreferencesService;
        private readonly ILogger<UserPreferencesController> _logger;
        
        public UserPreferencesController(UserPreferencesService userPreferencesService, ILogger<UserPreferencesController> logger)
        {
            _userPreferencesService = userPreferencesService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all user preferences.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("UserPreference GetAll called at {Time}", DateTime.UtcNow);

            String userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) 
            {
                return Unauthorized("You must be logged in to retrieve user preferences");
            }

            var userPreferences = await _userPreferencesService.GetAllAsync(userId);
            
            return Ok(userPreferences);
        }

        /// <summary>
        /// Gets a user preferences by its id.
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

            UserPreference userPreference = await _userPreferencesService.FindAsync(id, userId);

            if (userPreference == null)
            {
                return NotFound();
            }
            
            return Ok(userPreference);
        }

        /// <summary>
        /// Creates a user preferences.
        /// </summary>
        /// <param name="userPreferences"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserPreference userPreferences)
        {
            if (userPreferences == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid UserPreference data.");
            }
            
            await _userPreferencesService.AddAsync(userPreferences);
            
            return CreatedAtAction(nameof(GetById), new { id = userPreferences.Id }, userPreferences);
        }

        /// <summary>
        /// Updates a user preferences.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userPreferences"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserPreference userPreferences)
        {
            if (userPreferences == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid UserPreference data.");
            }
            
            var existingUserPreferences = await _userPreferencesService.FindAsync(userPreferences.Id);
            
            if (existingUserPreferences == null)
            {
                return NotFound();
            }
            
            await _userPreferencesService.UpdateAsync(userPreferences);
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a user preferences.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingUserPreferences = await _userPreferencesService.FindAsync(id);
            if (existingUserPreferences == null)
            {
                return NotFound();
            }

            if (existingUserPreferences.Timestamp != timestamp) 
            {
                return BadRequest("Concurrency check failed.");
            }

            await _userPreferencesService.DeleteAsync(existingUserPreferences);
            
            return NoContent();
        }
    }
}