using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;

namespace Application.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserAppService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return _userManager.Users.ToList();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> AddUserAsync(ApplicationUser user, string password, IEnumerable<string> roles)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(user, roles);
            }
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null) return false;

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            // Update other fields as necessary

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(existingUser);
                result = await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRolesAsync(existingUser, roles);
                }
            }
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return _roleManager.Roles.ToList();
        }
        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
}
}
