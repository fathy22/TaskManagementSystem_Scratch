using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace Application.Roles
{
    public interface IRolePermissionService
    {
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<IdentityResult> CreateRoleAsync(IdentityRole role);
        Task<IdentityResult> UpdateRoleAsync(IdentityRole role);
        Task<IdentityResult> DeleteRoleAsync(string roleId);

        Task<List<Claim>> GetAllPermissionsAsync();
        Task<IdentityResult> AddPermissionToRoleAsync(string roleId, string permission);
        Task<IdentityResult> RemovePermissionFromRoleAsync(string roleId, string permission);
        Task<IdentityResult> RemoveAllPermissionsByRoleAsync(string roleId);
        Task<List<string>> GetAllPermissionsByRoleIdAsync(string roleId);
    }
}
