using EduPath.Core.DTOs;
using EduPath.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduPath.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;


        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound("الكورس غير موجود");

            return Ok(course);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDto dto)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (instructorId == null)
                return Unauthorized();

            var course = await _courseService.CreateAsync(dto, instructorId);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateCourseDto dto)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (instructorId == null)
                return Unauthorized();

            var result = await _courseService.UpdateAsync(id, dto, instructorId);
            if (!result)
                return Forbid("انت مش صاحب هذا الكورس");

            return NoContent();
        }

        [Authorize(Roles = "Instructor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (instructorId == null)
                return Unauthorized();

            var result = await _courseService.DeleteAsync(id, instructorId);
            if (!result)
                return Forbid("انت مش صاحب هذا الكورس");

            return NoContent();
        }

        [Authorize(Roles = "Instructor")]
        [HttpPatch("{id}/publish")]
        public async Task<IActionResult> Publish(int id)
        {
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (instructorId == null)
                return Unauthorized();

            var result = await _courseService.PublishAsync(id, instructorId);
            if (!result)
                return Forbid("انت مش صاحب هذا الكورس");

            return Ok("تم نشر الكورس بنجاح");
        }
    }
}