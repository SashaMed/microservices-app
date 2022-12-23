using Identity.DbContexts;
using Identity.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.Initializers
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(StaticData.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(StaticData.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticData.Customer)).GetAwaiter().GetResult();
            }
            else return;
            var adminUser = new ApplicationUser
            {
                UserName = "admin_user",
                Email = "admin_user@gmail.com",
                PhoneNumberConfirmed = true,
                PhoneNumber = "1234567",
                FirstName = "sasha",
                LastName = "madness"
            };

            _userManager.CreateAsync(adminUser, "ZAQxsw123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, StaticData.Admin).GetAwaiter().GetResult();
            var admin = _userManager.AddClaimsAsync(adminUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, adminUser.FirstName + " " + adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                new Claim(JwtClaimTypes.Role, StaticData.Admin),
            }).GetAwaiter().GetResult();


            var customerUser = new ApplicationUser
            {
                UserName = "customer_user",
                Email = "customer_user@gmail.com",
                PhoneNumberConfirmed = true,
                PhoneNumber = "7654321",
                FirstName = "nesasha",
                LastName = "nemadness"
            };

            _userManager.CreateAsync(customerUser, "ZAQxsw123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, StaticData.Customer).GetAwaiter().GetResult();
            var customer = _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, customerUser.FirstName + " " + customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
                new Claim(JwtClaimTypes.Role, StaticData.Customer),
            }).GetAwaiter().GetResult();
        }
    }
}
