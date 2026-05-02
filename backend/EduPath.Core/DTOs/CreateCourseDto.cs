using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduPath.Core.DTOs
{
    public class CreateCourseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}