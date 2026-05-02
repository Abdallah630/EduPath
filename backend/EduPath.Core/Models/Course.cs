using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduPath.Core.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string InstructorId { get; set; } = string.Empty;
        public AppUser Instructor { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; } = false;
    }
}