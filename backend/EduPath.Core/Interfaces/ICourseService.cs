using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduPath.Core.DTOs;

namespace EduPath.Core.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponseDto>> GetAllAsync();
        Task<CourseResponseDto?> GetByIdAsync(int id);
        Task<CourseResponseDto> CreateAsync(CreateCourseDto dto, string instructorId);
        Task<bool> UpdateAsync(int id, CreateCourseDto dto, string instructorId);
        Task<bool> DeleteAsync(int id, string instructorId);
        Task<bool> PublishAsync(int id, string instructorId);
    }
}