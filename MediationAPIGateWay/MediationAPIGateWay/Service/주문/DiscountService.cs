using IdentityServer;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediationAPIGateWay.Service.주문
{
    public class DiscountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public DiscountService(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<decimal> CalculateDiscountAsync(IdentityUser user, decimal originalPrice)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var maxDiscount = 0m;

            foreach (var roleName in roles)
            {
                var role = await _dbContext.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
                if (role != null)
                {
                    var userRole = await _dbContext.UserRoles.SingleOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
                    if (userRole is ApplicationUserRole appUserRole)
                    {
                        maxDiscount = Math.Max(maxDiscount, appUserRole.DiscountRate);
                    }
                }
            }

            return originalPrice * (1 - maxDiscount / 100);
        }
    }
}
