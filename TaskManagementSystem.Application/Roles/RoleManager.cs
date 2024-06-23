using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
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
            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole == null)
            {
                adminRole = new IdentityRole("Admin");
                await roleManager.CreateAsync(adminRole);
            }

            // Get all current permissions for the Admin role
            var currentClaims = await roleManager.GetClaimsAsync(adminRole);

            // Get all defined permissions from the Permission class
            var permissionFields = typeof(Permission).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => fi.GetRawConstantValue() as string)
                .ToList();

            // Add new permissions to the Admin role
            foreach (var permission in permissionFields)
            {
                if (!currentClaims.Any(c => c.Type == permission))
                {
                    await roleManager.AddClaimAsync(adminRole, new Claim(permission, "true"));
                }
            }

            // Remove permissions that are no longer defined in the Permission class
            foreach (var claim in currentClaims)
            {
                if (!permissionFields.Contains(claim.Type))
                {
                    await roleManager.RemoveClaimAsync(adminRole, claim);
                }
            }

            var user = await userManager.FindByEmailAsync("admin@admin.com");
            if (user == null)
            {
                var defaultUser = new ApplicationUser
                {
                    FirstName = "admin",
                    SecondName = "admin",
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(defaultUser, "User@123");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                }
            }
        }
    }
}