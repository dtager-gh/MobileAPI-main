using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/Event")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SchoolEventController : ControllerBase
    {
        private readonly SchoolEventService _schoolEventService;
        private readonly ILogger<SchoolEventController> _logger;
        
        public SchoolEventController(SchoolEventService schoolEventService, ILogger<SchoolEventController> logger)
        {
            _schoolEventService = schoolEventService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all school events.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("SchoolEvent GetAll called at {Time}", DateTime.UtcNow);
            
            var schoolEvents = await _schoolEventService.GetAllAsync();
            
            return Ok(schoolEvents);
        }

        /// <summary>
        /// Gets a school event by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schoolEvent = await _schoolEventService.FindAsync(id);
            if (schoolEvent == null)
            {
                return NotFound();
            }
            
            return Ok(schoolEvent);
        }

        /// <summary>
        /// Creates a school event.
        /// </summary>
        /// <param name="schoolEvent"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SchoolEvent schoolEvent)
        {
            if (schoolEvent == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid SchoolEvent data.");
            }
            
            await _schoolEventService.AddAsync(schoolEvent);
            
            return CreatedAtAction(nameof(GetById), new { id = schoolEvent.Id }, schoolEvent);
        }

        /// <summary>
        /// Updates a school event.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="schoolEvent"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] SchoolEvent schoolEvent)
        {
            if (schoolEvent == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid SchoolEvent data.");
            }
            
            var existingSchoolEvent = await _schoolEventService.FindAsync(schoolEvent.Id);
            if (existingSchoolEvent == null)
            {
                return NotFound();
            }

            if (existingSchoolEvent.Timestamp != schoolEvent.Timestamp)
            {
                return BadRequest("Concurrency check failed");
            }

            await _schoolEventService.UpdateAsync(schoolEvent);
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a school  event.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingSchoolEvent = await _schoolEventService.FindAsync(id);
            if (existingSchoolEvent == null)
            {
                return NotFound();
            }

            if (existingSchoolEvent.Timestamp != timestamp) 
            {
                return BadRequest("Concurrency check failed.");
            }

            await _schoolEventService.DeleteAsync(existingSchoolEvent);
            
            return NoContent();
        }
    }
}