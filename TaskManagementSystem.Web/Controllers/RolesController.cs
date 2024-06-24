using Application.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementSystem.Web.Models;

[Authorize(Roles = "Admin")] 
public class RolesController : Controller
{
    private readonly IRolePermissionService _rolePermissionService;

    public RolesController(IRolePermissionService rolePermissionService)
    {
        _rolePermissionService = rolePermissionService;
    }

    public async Task<IActionResult> Index()
    {
        var roles = await _rolePermissionService.GetAllRolesAsync();
        return View(roles);
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = new CreateRoleViewModel
        {
            Permissions = _rolePermissionService.GetAllPermissionsAsync().Result
                .Select(p => new PermissionViewModel { Value = p.Value })
                .ToList()
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var role = new IdentityRole { Name = viewModel.Name };
            var result = await _rolePermissionService.CreateRoleAsync(role);

            if (result.Succeeded)
            {
                if (viewModel.SelectedPermissions != null)
                {
                    foreach (var permission in viewModel.SelectedPermissions)
                    {
                        await _rolePermissionService.AddPermissionToRoleAsync(role.Id, permission);
                    }
                }
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        viewModel.Permissions = _rolePermissionService.GetAllPermissionsAsync().Result
            .Select(p => new PermissionViewModel { Value = p.Value })
            .ToList();

        return View(viewModel);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var role = await _rolePermissionService.GetRoleByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        var viewModel = new RoleViewModel
        {
            Id = role.Id,
            Name = role.Name,
            SelectedPermissions = (await _rolePermissionService.GetAllPermissionsByRoleIdAsync(role.Id))
                .Select(p => p)
                .ToList(),
            Permissions = _rolePermissionService.GetAllPermissionsAsync().Result
                .Select(p => new PermissionViewModel { Value = p.Value })
                .ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, RoleViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var role = await _rolePermissionService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            role.Name = viewModel.Name;
            var result = await _rolePermissionService.UpdateRoleAsync(role);

            if (result.Succeeded)
            {
                await _rolePermissionService.RemoveAllPermissionsByRoleAsync(role.Id);
                if (viewModel.SelectedPermissions != null)
                {
                    foreach (var permission in viewModel.SelectedPermissions)
                    {
                        await _rolePermissionService.AddPermissionToRoleAsync(role.Id, permission);
                    }
                }
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        viewModel.Permissions = _rolePermissionService.GetAllPermissionsAsync().Result
            .Select(p => new PermissionViewModel { Value = p.Value })
            .ToList();

        return View(viewModel);
    }

    public async Task<IActionResult> Delete(string id)
    {
        var role = await _rolePermissionService.GetRoleByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var result = await _rolePermissionService.DeleteRoleAsync(id);
        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }
        TempData["ErrorMessage"] = "Role deletion failed.";
        return RedirectToAction("Index");
    }
}
