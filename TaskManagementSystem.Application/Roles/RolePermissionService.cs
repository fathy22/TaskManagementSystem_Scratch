using Application.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;
namespace Application.Roles
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolePermissionService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<IdentityResult> CreateRoleAsync(IdentityRole role)
        {
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole role)
        {
            var existingRole = await _roleManager.FindByIdAsync(role.Id);
            existingRole.Name = role.Name;
            return await _roleManager.UpdateAsync(existingRole);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<List<Claim>> GetAllPermissionsAsync()
        {
            var claims = await _roleManager.GetClaimsAsync(new IdentityRole());
            return claims.ToList();
        }
        public async Task<List<string>> GetAllPermissionsByRoleIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new ApplicationException($"Role with ID {roleId} not found.");
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var permissions = roleClaims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();

            return permissions;
        }

        public async Task<IdentityResult> AddPermissionToRoleAsync(string roleId, string permission)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var claim = new Claim("Permission", permission);
            return await _roleManager.AddClaimAsync(role, claim);
        }

        public async Task<IdentityResult> RemovePermissionFromRoleAsync(string roleId, string permission)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var claim = new Claim("Permission", permission);
            return await _roleManager.RemoveClaimAsync(role, claim);
        }
        public async Task<IdentityResult> RemoveAllPermissionsByRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new ApplicationException($"Role with ID {roleId} not found.");
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in roleClaims.Where(c => c.Type == "Permission"))
            {
                var result = await _roleManager.RemoveClaimAsync(role, claim);
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            return IdentityResult.Success;
        }

    }
}
