using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/Registrar")]
    [ApiController]
    [ApiVersion("1.0")]
    public class RegistrarController : ControllerBase
    {
        private readonly RegistrarService _registrarService;
        private readonly ILogger<RegistrarController> _logger;

        public RegistrarController(RegistrarService registrarService, ILogger<RegistrarController> logger)
        {
            _registrarService = registrarService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all resources registrar.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Registrar GetAll called at {Time}", DateTime.UtcNow);
            
            var registrars = await _registrarService.GetAllAsync();

            return Ok(registrars);
        }

        /// <summary>
        /// Gets a registrar by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var registrar = await _registrarService.FindAsync(id);
            if (registrar == null)
            {
                return NotFound();
            }
            
            return Ok(registrar);
        }

        /// <summary>
        /// Creates a registrar.
        /// </summary>
        /// <param name="registrar"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Registrar registrar)
        {
            if (registrar == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Registrar data.");
            }

            await _registrarService.AddAsync(registrar);
                        
            return CreatedAtAction(nameof(GetById), new { id = registrar.Id }, registrar);
        }

        /// <summary>
        /// Updates a registrar.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="registrar"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Registrar registrar)
        {
            if (registrar == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Registrar data.");
            }
            
            var existingRegistrar = await _registrarService.FindAsync(registrar.Id);
            if (existingRegistrar == null)
            {
                return NotFound();
            }

            if (existingRegistrar.Timestamp != registrar.Timestamp) 
            {
                return BadRequest("Concurrency check failed");
            }

            await _registrarService.UpdateAsync(registrar);

            return NoContent();
        }

        /// <summary>
        /// Deletes a registrar.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingRegistrar = await _registrarService.FindAsync(id);
            if (existingRegistrar == null)
            {
                return NotFound();
            }

            if (existingRegistrar.Timestamp != timestamp) 
            {
                return BadRequest("Concurrency check failed.");
            }

            await _registrarService.DeleteAsync(existingRegistrar);

            return NoContent();
        }
    }
}