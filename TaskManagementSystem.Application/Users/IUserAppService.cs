using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;

namespace Application.Users
{
    public interface IUserAppService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<bool> AddUserAsync(ApplicationUser user, string password, IEnumerable<string> roles);
        Task<bool> UpdateUserAsync(ApplicationUser user, IEnumerable<string> roles);
        Task<bool> DeleteUserAsync(string userId);
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        string GetAdminUserId();
        Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName);
    }
}
