using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/Library")]
    [ApiController]
    [ApiVersion("1.0")]
    public class LibraryController : ControllerBase
    {
        private readonly LibraryService _libraryService;
        private readonly ILogger<LibraryController> _logger;

        public LibraryController(LibraryService libraryService, ILogger<LibraryController> logger)
        {
            _libraryService = libraryService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all resources librarys.
        /// </summary>
        /// <returns>A list of all libraries</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("{Class}.GetAll() called at {Time}", this.GetType().Name, DateTime.UtcNow);
            
            var librarys = await _libraryService.GetAllAsync();
            
            return Ok(librarys);
        }

        /// <summary>
        /// Gets a library by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var library = await _libraryService.FindAsync(id);
            if (library == null)
            { 
                return NotFound();
            }

            return Ok(library);
        }

        /// <summary>
        /// Creates a library.
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Library library)
        {
            if (library == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Library data.");
            }

            await _libraryService.AddAsync(library);
            
            return CreatedAtAction(nameof(GetById), new { id = library.Id }, library);
        }

        /// <summary>
        /// Updates a library.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="library"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Library library)
        {
            if (library == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Library data.");
            }

            var existingLibrary = await _libraryService.FindAsync(library.Id);
            if (existingLibrary == null)
            {
                return NotFound();
            }

            if (existingLibrary.Timestamp != library.Timestamp) 
            {
                return BadRequest("Concurrency check failed");
            }

            await _libraryService.UpdateAsync(library);
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a library.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingLibrary = await _libraryService.FindAsync(id);
            if (existingLibrary == null)
            {
                return NotFound();
            }

            if (existingLibrary.Timestamp != timestamp) 
            {
                return BadRequest("Concurrency check failed.");
            }

            await _libraryService.DeleteAsync(existingLibrary);
            
            return NoContent();
        }
    }
}