using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduPath.Core.DTOs;
using EduPath.Core.Interfaces;
using EduPath.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduPath.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<AppUser> userManger, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManger = userManger;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new AppUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email
            };
            var result = await _userManger.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            await _userManger.AddToRoleAsync(user, dto.Role);
            return Ok("تم التسجيل بنجاح");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManger.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("بيانات غير صحيحة!");
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Unauthorized("بيانات غير صحيحة!");

            var roles = await _userManger.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Student";
            var token = _tokenService.GenerateToken(user, role);
            return Ok(new AuthResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email!,
                Role = role
            });
        }
    }

}