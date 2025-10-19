using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/Announcement")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AnnouncementController : ControllerBase
    {
        private readonly AnnouncementService _announcementService;
        private readonly ILogger<AnnouncementController> _logger;

        public AnnouncementController(AnnouncementService announcementService, ILogger<AnnouncementController> logger)
        {
            _announcementService = announcementService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all school announcements.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Announcement GetAll called at {Time}", DateTime.UtcNow);

            var schoolAnnouncements = await _announcementService.GetAllAsync();

            return Ok(schoolAnnouncements);
        }

        /// <summary>
        /// Gets a school announcement by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var announcement = await _announcementService.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            
            return Ok(announcement);
        }

        /// <summary>
        /// Creates an announcement.
        /// </summary>
        /// <param name="announcement"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Announcement announcement)
        {
            if (announcement == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Announcement data.");
            }
            
            await _announcementService.AddAsync(announcement);

            return CreatedAtAction(nameof(GetById), new { id = announcement.Id }, announcement);
        }

        /// <summary>
        /// Updates a school announcement.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="announcement"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Announcement announcement)
        {
            if (announcement == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Announcement data.");
            }
            
            var existingSchoolAnnouncement = await _announcementService.FindAsync(announcement.Id);
            if (existingSchoolAnnouncement == null)
            {
                return NotFound();
            }

            if (existingSchoolAnnouncement.Timestamp != announcement.Timestamp)
            {
                return BadRequest("Concurrency check failed");
            }
            
            await _announcementService.UpdateAsync(announcement);
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a school announcement.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingAnnouncement = await _announcementService.FindAsync(id);

            if (existingAnnouncement == null)
            {
                return NotFound();
            }

            if (existingAnnouncement.Timestamp != timestamp) 
            {
                return BadRequest("Concurrency check failed.");
            }
            
            await _announcementService.DeleteAsync(existingAnnouncement);
            
            return NoContent();
        }
    }
}