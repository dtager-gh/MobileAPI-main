using System.ComponentModel.DataAnnotations;

namespace MobileAPI.Models
{
    public class Campus : EntityBase
    {
        public Campus() 
        { 
            Name = string.Empty;
            Address = new Address();
            School = new School();
        }

        [Required]
        public string Name { get; set; }

        public int AddressId {  get; set; }
        public Address Address { get; set; }
        public int SchoolId { get; set; }
        public School School { get; set; }
    }
}
