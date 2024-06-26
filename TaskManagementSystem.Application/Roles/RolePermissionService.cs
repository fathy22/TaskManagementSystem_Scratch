using Application.CustomLogs;
using Application.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Permissions;
using TaskManagementSystem.Core.Sessions;

namespace Application.Roles
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomLogAppService _customLogAppService;
        private readonly ICustomSession _customSession;

        public RolePermissionService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ICustomLogAppService customLogAppService, ICustomSession customSession)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _customLogAppService = customLogAppService;
            _customSession = customSession;
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
            var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
            await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
            {
                Description = $"{user.FirstName} {user.SecondName} create New role"
            });
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole role)
        {
            var existingRole = await _roleManager.FindByIdAsync(role.Id);
            existingRole.Name = role.Name;
            var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
            await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
            {
                Description = $"{user.FirstName} {user.SecondName} update role"
            });
            return await _roleManager.UpdateAsync(existingRole);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
            await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
            {
                Description = $"{user.FirstName} {user.SecondName} delete role"
            });
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<List<string>> GetAllPermissionsAsync()
        {
            //var claims = await _roleManager.GetClaimsAsync(new IdentityRole());
            //return claims.ToList();
            var permissionFields = typeof(Permission).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
               .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
               .Select(fi => fi.GetRawConstantValue() as string)
               .ToList();
            return permissionFields;
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
