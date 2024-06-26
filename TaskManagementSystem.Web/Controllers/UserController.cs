using Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserAppService _userService;

        public UserController(IUserAppService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var users = _userService.GetAllUsersAsync().Result;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adminUserId = _userService.GetAdminUserId();

            var viewModel = new UserListViewModel
            {
                Users = users,
                CurrentUserId = currentUserId,
                AdminUserId = adminUserId
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Create()
        {
            var roles = await _userService.GetAllRolesAsync();
            ViewBag.Roles = roles;
            return View(new CreateUserViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    EmailConfirmed=true
                };
                var result = await _userService.AddUserAsync(user, model.Password, model.SelectedRoles);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var roles = await _userService.GetAllRolesAsync();
            ViewBag.Roles = roles;
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userService.GetAllRolesAsync();
            var selectedRoles = await _userService.GetUserRolesAsync(user);

            var model = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                SelectedRoles = selectedRoles.ToList()
            };

            ViewBag.Roles = roles;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.SecondName = model.SecondName;

                var result = await _userService.UpdateUserAsync(user, model.SelectedRoles);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var roles = await _userService.GetAllRolesAsync();
            ViewBag.Roles = roles;
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }
    }
}
