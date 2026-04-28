using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduPath.Core.Models;

namespace EduPath.Core.Interfaces
{
    public interface ITokenService
    {
      public string GenerateToken(AppUser user,string role);
    }
}