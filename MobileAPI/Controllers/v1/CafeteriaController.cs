using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/Cafeteria")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CafeteriaController : ControllerBase
    {
        private readonly ILogger<CafeteriaController> _logger;
        private readonly CafeteriaSpecialService _cafeteriaSpecialService;
        private readonly CafeteriaMenuService _cafeteriaMenuService;
     
        public CafeteriaController(ILogger<CafeteriaController> logger, CafeteriaSpecialService cafeteriaSpecialService, CafeteriaMenuService cafeteriaMenuService)
        {
            _logger = logger;
            _cafeteriaSpecialService = cafeteriaSpecialService;
            _cafeteriaMenuService = cafeteriaMenuService;
        }

        /// <summary>
        /// Displays an image of today's cafeteria special.
        /// </summary>
        /// <returns></returns>
        [Route("Special")]
        [HttpGet]
        public IActionResult GetTodaysSpecial()
        {
            _logger.LogInformation("GetTodaysSpecial called at {Time}", DateTime.UtcNow);

            FileStream fileStream = _cafeteriaSpecialService.GetTodaysSpecial();

            // Force browser to not cache result since filename is re-used each week
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, proxy-revalidate";
            Response.Headers["Expires"] = "0";
            Response.Headers["Pragma"] = "no-cache";

            return File(fileStream, "image/jpg");
        }

        /// <summary>
        /// Use to upload a cafeteria special image file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        [Route("Special")]
        [HttpPost]
        public async Task<IActionResult> UploadSpecial(IFormFile file, [FromQuery] string dayOfWeek)
        {
            _logger.LogInformation("UploadSpecial called at {Time}", DateTime.UtcNow);
            if (file == null || file.Length == 0)
            {
                return Content("file not selected");
            }

            // TryParse 2nd parameter makes this case insensitive
            bool isValidDay = Enum.TryParse(dayOfWeek, true, out DayOfWeek day);
            if (!isValidDay)
            {
                return BadRequest("Invalid day of the week.");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            return Ok(new { message = $"Image for {dayOfWeek} uploaded successfully.", path });
        }

        /// <summary>
        /// Use to delete a cafeteria special image file.
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        [Route("Special")]
        [HttpDelete]
        public IActionResult DeleteSpecial([FromQuery] string dayOfWeek)
        {
            _logger.LogInformation("DeleteSpecial called at {Time}", DateTime.UtcNow);

            // TryParse 2nd parameter makes this ignore case. 
            bool isValidDay = Enum.TryParse(dayOfWeek, true, out DayOfWeek day);
            if (!isValidDay) 
            {
                return BadRequest("Invalid day of the week.");
            }

            string fileName = $"{dayOfWeek}.png";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

            if (!System.IO.File.Exists(path))
            {
                return NotFound($"No image found for {dayOfWeek}.");
            }

            try
            {
                System.IO.File.Delete(path);
                return NoContent();
            }
            catch (IOException ex)
            {
                return StatusCode(500, $"An error occurred while deleting the file: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all cafeteria menus.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Cafeteria GetAll called at {Time}", DateTime.UtcNow);

            var cafeteriaMenus = await _cafeteriaMenuService.GetAllAsync();

            return Ok(cafeteriaMenus);
        }

        /// <summary>
        /// Gets a menu by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cafeteriaMenu = await _cafeteriaMenuService.FindAsync(id);
            if (cafeteriaMenu == null)
            {
                return NotFound();
            }

            return Ok(cafeteriaMenu);
        }

        /// <summary>
        /// Creates a cafeteria menu.
        /// </summary>
        /// <param name="cafeteriaMenu"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CafeteriaMenu cafeteriaMenu)
        {
            if (cafeteriaMenu == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid cafeteriaMenu data.");
            }

            await _cafeteriaMenuService.AddAsync(cafeteriaMenu);
            return CreatedAtAction(nameof(GetById), new { id = cafeteriaMenu.Id }, cafeteriaMenu);
        }

        /// <summary>
        /// Updates a cafeteria menu.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cafeteriaMenu"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CafeteriaMenu cafeteriaMenu)
        {
            if (cafeteriaMenu == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid cafeteriaMenu data.");
            }

            var existingCafeteriaMenu = await _cafeteriaMenuService.FindAsync(cafeteriaMenu.Id);
            if (existingCafeteriaMenu == null)
            {
                return NotFound();
            }

            if (existingCafeteriaMenu.Timestamp != cafeteriaMenu.Timestamp)
            {
                return BadRequest("Concurrency check failed");
            }

            await _cafeteriaMenuService.UpdateAsync(cafeteriaMenu);

            return NoContent();
        }

        /// <summary>
        /// Deletes a cafeteria menu.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingCafeteriaMenu = await _cafeteriaMenuService.FindAsync(id);

            if (existingCafeteriaMenu == null)
            {
                return NotFound();
            }

            if (existingCafeteriaMenu.Timestamp != timestamp)
            {
                return BadRequest("Concurrency check failed.");
            }

            await _cafeteriaMenuService.DeleteAsync(existingCafeteriaMenu);

            return NoContent();
        }
    }
}