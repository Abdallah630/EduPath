
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduPath.Core.Interfaces;
using EduPath.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EduPath.Application.Services
{
    public class TokenService : ITokenService
    {
            private readonly IConfiguration _configuration;

            public TokenService(IConfiguration configuration)
            {
                _configuration = configuration;
            }
        public string GenerateToken(AppUser user, string role)
        {
                //Private Claims (User-Defined)
            var claims = new List<Claim>()
            {
             new Claim(ClaimTypes.NameIdentifier, user.Id),   
             new Claim(ClaimTypes.Email, user.Email!),
             new Claim(ClaimTypes.Name,user.FullName),   
             new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(_configuration["JwtSettings:ExpiryMinutes"]!)),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
    
    }
}