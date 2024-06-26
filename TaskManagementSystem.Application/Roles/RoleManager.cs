using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementSystem.Authorization.Roles;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Permissions;

namespace TaskManagementSystem.Application.Roles
{
    public static class RoleManager
    {
        public static async Task SeedRolesAndUsers(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Ensure Admin Role exists
            var adminRole = await roleManager.FindByNameAsync(StaticRoleNames.Host.Admin);
            var teamLeadRole = await roleManager.FindByNameAsync(StaticRoleNames.Host.TeamLeads);
            var regularUserRole = await roleManager.FindByNameAsync(StaticRoleNames.Host.RegularUsers);
            if (adminRole == null)
            {
                adminRole = new IdentityRole(StaticRoleNames.Host.Admin);
                await roleManager.CreateAsync(adminRole);
            }
            if (teamLeadRole == null)
            {
                teamLeadRole = new IdentityRole(StaticRoleNames.Host.TeamLeads);
                await roleManager.CreateAsync(teamLeadRole);
            }
            if (regularUserRole == null)
            {
                regularUserRole = new IdentityRole(StaticRoleNames.Host.RegularUsers);
                await roleManager.CreateAsync(regularUserRole);
            }

            // Get all current permissions for the Admin role
            var currentClaims = await roleManager.GetClaimsAsync(adminRole);

            // Get all defined permissions from the Permission class
            var permissionFields = typeof(Permission).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => fi.GetRawConstantValue() as string)
                .ToList();
            if (permissionFields.Any(x => x.Contains("My Tasks Management")))
            {
                var per = permissionFields.FirstOrDefault(x => x.Contains("My Tasks Management"));
                permissionFields.Remove(per);
            }
            if (permissionFields.Any(x => x.Contains("Team Tasks Management")))
            {
                var per = permissionFields.FirstOrDefault(x => x.Contains("Team Tasks Management"));
                permissionFields.Remove(per);
            }
            // Add new permissions to the Admin role
            foreach (var permission in permissionFields)
            {
                if (!currentClaims.Any(c => c.Value == permission))
                {
                    await roleManager.AddClaimAsync(adminRole, new Claim("Permission",permission));
                }
            }

            // Remove permissions that are no longer defined in the Permission class
            foreach (var claim in currentClaims)
            {
                if (!permissionFields.Contains(claim.Value))
                {
                    await roleManager.RemoveClaimAsync(adminRole, claim);
                }
            }

            var user = await userManager.FindByEmailAsync("admin@admin.com");
            if (user == null)
            {
                var defaultUser = new ApplicationUser
                {
                    FirstName = StaticRoleNames.Host.Admin,
                    SecondName = StaticRoleNames.Host.Admin,
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(defaultUser, "User@123");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, StaticRoleNames.Host.Admin);
                }
            }
        }
    }
}