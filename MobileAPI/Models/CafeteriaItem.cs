using System.ComponentModel.DataAnnotations;

namespace MobileAPI.Models
{
    public enum MenuCategory
    { 
        Side,
        Drink,
        Appetizer
    }
    public class CafeteriaItem : EntityBase
    {
        public CafeteriaItem() 
        { 
            Name = string.Empty;
            Description = string.Empty;
            IconName = string.Empty;
        }
        
        public MenuCategory Category { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        // This holds the Material icon name (Icons.favorite, etc.)
        public string IconName { get; set; }
    }
}
