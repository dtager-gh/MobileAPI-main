using System;

namespace MobileAPI.Models
{
    public class SchoolNews : EntityBase
    {
        public string Headline { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DatePublished { get; set; }
        public string Author { get; set; } = string.Empty;
        public int SchoolId { get; set; }
        public School School { get; set; } = null!;
    }
}