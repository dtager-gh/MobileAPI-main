using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/News")]
    [ApiController]
    [ApiVersion("1.0")]
    public class NewsController : ControllerBase
    {
        private readonly NewsService _newsService;
        private readonly ILogger<NewsController> _logger;
        
        public NewsController(NewsService newsService, ILogger<NewsController> logger)
        {
            _newsService = newsService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all school news.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("SchoolNews GetAll called at {Time}", DateTime.UtcNow);
            
            var schoolNews = await _newsService.GetAllAsync();
            
            return Ok(schoolNews);
        }

        /// <summary>
        /// Gets a school news by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schoolNews = await _newsService.FindAsync(id);
            if (schoolNews == null)
            {
                return NotFound();
            }
            
            return Ok(schoolNews);
        }

        /// <summary>
        /// Creates a school news.
        /// </summary>
        /// <param name="schoolNews"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SchoolNews schoolNews)
        {
            if (schoolNews == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid SchoolNews data.");
            }
            
            await _newsService.AddAsync(schoolNews);
            
            return CreatedAtAction(nameof(GetById), new { id = schoolNews.Id }, schoolNews);
        }

        /// <summary>
        /// Updates a school news.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="schoolNews"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] SchoolNews schoolNews)
        {
            if (schoolNews == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid SchoolNews data.");
            }
            
            var existingSchoolNews = await _newsService.FindAsync(schoolNews.Id);
            if (existingSchoolNews == null)
            {
                return NotFound();
            }

            if (existingSchoolNews.Timestamp != schoolNews.Timestamp)
            {
                return BadRequest("Concurrency check failed.");
            }

            await _newsService.UpdateAsync(schoolNews);
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a school news.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingSchoolNews = await _newsService.FindAsync(id);
            if (existingSchoolNews == null)
            {
                return NotFound();
            }

            if (existingSchoolNews.Timestamp != timestamp)
            {
                return BadRequest("Concurrency check failed.");
            }
            
            await _newsService.DeleteAsync(existingSchoolNews);
            
            return NoContent();
        }
    }
}