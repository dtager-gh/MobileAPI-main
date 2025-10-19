using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/BusinessOffice")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BusinessOfficeController : ControllerBase
    {
        private readonly BusinessOfficeService _businessOfficeService;
        private readonly ILogger<BusinessOfficeController> _logger;

        public BusinessOfficeController(BusinessOfficeService businessOfficeService, ILogger<BusinessOfficeController> logger)
        {
            _businessOfficeService = businessOfficeService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all business offices.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("BusinessOffice GetAll called at {Time}", DateTime.UtcNow);
            
            var businessOffices = await _businessOfficeService.GetAllAsync();
            
            return Ok(businessOffices);
        }

        /// <summary>
        /// Gets a business office by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var businessOffice = await _businessOfficeService.FindAsync(id);
            if (businessOffice == null)
            {
                return NotFound();
            }
            
            return Ok(businessOffice);
        }

        /// <summary>
        /// Creates a business office.
        /// </summary>
        /// <param name="businessOffice"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessOffice businessOffice)
        {
            if (businessOffice == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid businessOffice data.");
            }
            
            await _businessOfficeService.AddAsync(businessOffice);
            return CreatedAtAction(nameof(GetById), new { id = businessOffice.Id }, businessOffice);
        }

        /// <summary>
        /// Updates a business office.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="businessOffice"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] BusinessOffice businessOffice)
        {
            if (businessOffice == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid businessOffice data.");
            }
            
            var existingBusinessOffice = await _businessOfficeService.FindAsync(businessOffice.Id);
            if (existingBusinessOffice == null)
            {
                return NotFound();
            }

            if (existingBusinessOffice.Timestamp != businessOffice.Timestamp) 
            {
                return BadRequest("Concurrency check failed");
            }

            await _businessOfficeService.UpdateAsync(businessOffice);

            return NoContent();
        }

        /// <summary>
        /// Deletes a business office.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingBusinessOffice = await _businessOfficeService.FindAsync(id);

            if (existingBusinessOffice == null)
            {
                return NotFound();
            }

            if (existingBusinessOffice.Timestamp != timestamp) 
            {
                return BadRequest("Concurrency check failed.");
            }
            
            await _businessOfficeService.DeleteAsync(existingBusinessOffice);

            return NoContent();
        }
    }
}