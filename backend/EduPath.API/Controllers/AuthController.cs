using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduPath.Core.DTOs;
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

        public AuthController(UserManager<AppUser> userManger, SignInManager<AppUser> signInManager)
        {
            _userManger = userManger;
            _signInManager = signInManager;
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
            var result = await _userManger.CreateAsync(user,dto.Password);
            if(!result.Succeeded)
            return BadRequest(result.Errors);
            await _userManger.AddToRoleAsync(user,dto.Role);
            return Ok("تم التسجيل بنجاح");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await  _userManger.FindByEmailAsync(dto.Email);
                if(user == null)
                 return Unauthorized("بيانات غير صحيحة!");
            var result = await _signInManager.CheckPasswordSignInAsync(user,dto.Password,false);
                if(!result.Succeeded)
                 return Unauthorized("بيانات غير صحيحة!");

            return Ok("تم الدخول بنجاح");
        }
    }

}