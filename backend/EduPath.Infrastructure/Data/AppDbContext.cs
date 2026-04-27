using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using  EduPath.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EduPath.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {
            
        }
    }
}