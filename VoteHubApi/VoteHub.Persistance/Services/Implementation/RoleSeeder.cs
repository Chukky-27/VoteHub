using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VoteHub.Domain.Enums;
using VoteHub.Persistance.Services.Interfaces;

namespace VoteHub.Persistance.Services.Implementation
{
    public class RoleSeeder : IRoleSeeder
    {
        private readonly IServiceProvider _serviceProvider;

        public RoleSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SeedRolesAsync()
        {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }
        }
    }
}
