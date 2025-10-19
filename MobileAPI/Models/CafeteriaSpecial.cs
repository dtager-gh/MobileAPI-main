using System;
using System.ComponentModel.DataAnnotations;

namespace MobileAPI.Models
{
    public class CafeteriaSpecial
    {
        public CafeteriaSpecial() 
        { 
            ImagePath = String.Empty;
        }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        
        [Required]
        public string ImagePath { get; set; }
    }
}