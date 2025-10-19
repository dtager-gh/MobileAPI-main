using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace MobileAPI.Services
{
    public class CafeteriaSpecialService
    {
        private ILogger<CafeteriaSpecialService> _logger;
        public CafeteriaSpecialService(ILogger<CafeteriaSpecialService> logger)
        {
            _logger = logger;
        }

        public FileStream GetTodaysSpecial()
        {
            string today = DateTime.Today.DayOfWeek.ToString();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", $"{today}.jpg");
            FileStream fileStream;

            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogError("File not found. Path: " + filePath);
                return null;
            }

            try
            {
                fileStream = System.IO.File.OpenRead(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

            return fileStream;
        }
    }
}