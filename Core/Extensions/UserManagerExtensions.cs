using Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> GetUserByEmailByClaimsPrincipalWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            return await userManager.Users.Include(x => x.Address)
                .SingleOrDefaultAsync(u => u.Email == user.FindFirstValue(ClaimTypes.Email));
        }

        public static async Task<AppUser> GetUserByEmailByClaimsPrincipal(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            return await userManager.Users.SingleOrDefaultAsync(u => u.Email == user.FindFirstValue(ClaimTypes.Email));
        }
    }
}
