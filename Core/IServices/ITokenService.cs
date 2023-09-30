using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Identity;

namespace Core.IServices
{
    public interface ITokenService
    {
        public string CreateToken(AppUser appUser);
    }
}