using Application.CustomLogs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Sessions;

namespace Application.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomSession _customSession;
        private readonly ICustomLogAppService _customLogAppService;
        public UserAppService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ICustomLogAppService customLogAppService, ICustomSession customSession)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _customLogAppService = customLogAppService;
            _customSession = customSession;
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
                if (_customSession.UserId != null)
                {
                    var usr = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
                    await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
                    {
                        Description = $"{usr.FirstName} {usr.SecondName} add new user"
                    });
                }
            }
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null) return false;

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.SecondName = user.SecondName;

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(existingUser);
                result = await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRolesAsync(existingUser, roles);
                }
                if (_customSession.UserId != null)
                {
                    var usr = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
                    await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
                    {
                        Description = $"{usr.FirstName} {usr.SecondName} update user"
                    });
                }
            }
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            if (_customSession.UserId != null)
            {
                var usr = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
                await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
                {
                    Description = $"{usr.FirstName} {usr.SecondName} delete user"
                });
            }
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
        public string GetAdminUserId()
        {
            var adminUser = _userManager.GetUsersInRoleAsync("Admin").Result.FirstOrDefault();
            return adminUser?.Id;
        }
        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new ArgumentException("Role does not exist.");
            }
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.ToList();
        }

    }
}
