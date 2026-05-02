using EduPath.Core.DTOs;
using EduPath.Core.Interfaces;
using EduPath.Core.Models;
using EduPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPath.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseResponseDto>> GetAllAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.IsPublished)
                .ToListAsync();

            return courses.Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                InstructorName = c.Instructor.FullName,
                IsPublished = c.IsPublished,
                CreatedAt = c.CreatedAt
            });
        }

        public async Task<CourseResponseDto?> GetByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return null;

            return new CourseResponseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                InstructorName = course.Instructor.FullName,
                IsPublished = course.IsPublished,
                CreatedAt = course.CreatedAt
            };
        }

        public async Task<CourseResponseDto> CreateAsync(CreateCourseDto dto, string instructorId)
        {
            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                InstructorId = instructorId,
                IsPublished = false
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var instructor = await _context.Users.FindAsync(instructorId);

            return new CourseResponseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                InstructorName = instructor!.FullName,
                IsPublished = course.IsPublished,
                CreatedAt = course.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateCourseDto dto, string instructorId)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null || course.InstructorId != instructorId)
                return false;

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.Price = dto.Price;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, string instructorId)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null || course.InstructorId != instructorId)
                return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> PublishAsync(int id, string instructorId)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null || course.InstructorId != instructorId)
                return false;

            course.IsPublished = true;
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}