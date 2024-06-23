using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Permissions;
using TaskManagementSystem.Web.Models;

public class RolesController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_roleManager.Roles);
    }

    public IActionResult Create()
    {
        var permissions = GetAllPermissions();
        var viewModel = new CreateRoleViewModel
        {
            Permissions = permissions.Select(p => new PermissionViewModel
            {
                Name = p,
                IsSelected = false
            }).ToList()
        };
        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleViewModel model)
    {
        var permissions = GetAllPermissions();
        if (ModelState.IsValid)
        {
            var role = new IdentityRole(model.RoleName);
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                await UpdateRolePermissions(role, model.Permissions);
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        model.Permissions = permissions.Select(p => new PermissionViewModel
        {
            Name = p,
            IsSelected = model.Permissions.Any(mp => mp.Name == p && mp.IsSelected) // Preserve selection
        }).ToList();
        return View(model);
    }
    public IActionResult Edit(string id)
    {
        var role = _roleManager.FindByIdAsync(id).Result;
        if (role == null)
        {
            return NotFound();
        }
        var permissions = GetAllPermissions();
        var model = new RoleViewModel
        {
            RoleId = role.Id,
            RoleName = role.Name,
            Permissions = permissions.Select(p => new PermissionViewModel
            {
                Name = p,
                IsSelected = _roleManager.GetClaimsAsync(role).Result.Any(c => c.Type == "Permission" && c.Value == p)
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RoleViewModel model)
    {
        var role = await _roleManager.FindByIdAsync(model.RoleId);
        if (role == null)
        {
            return NotFound();
        }

        role.Name = model.RoleName;

        // Update permissions based on checkbox selection
        foreach (var permission in model.Permissions)
        {
            var existingClaim = await _roleManager.GetClaimsAsync(role);
            if (permission.IsSelected && !existingClaim.Any(c => c.Type == "Permission" && c.Value == permission.Name))
            {
                await _roleManager.AddClaimAsync(role, new Claim("Permission", permission.Name));
            }
            else if (!permission.IsSelected && existingClaim.Any(c => c.Type == "Permission" && c.Value == permission.Name))
            {
                var claimToRemove = existingClaim.FirstOrDefault(c => c.Type == "Permission" && c.Value == permission.Name);
                await _roleManager.RemoveClaimAsync(role, claimToRemove);
            }
        }

        var result = await _roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    public IActionResult Delete(string id)
    {
        var role = _roleManager.FindByIdAsync(id).Result;
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(role);
    }
    private async Task UpdateRolePermissions(IdentityRole role, List<PermissionViewModel> permissions)
    {
        foreach (var permission in permissions)
        {
            if (permission.IsSelected)
            {
                await _roleManager.AddClaimAsync(role, new Claim("Permission", permission.Name));
            }
        }
    }
    private List<string?> GetAllPermissions()
    {
        return typeof(Permission).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
            .Select(fi => fi.GetRawConstantValue() as string)
            .ToList();
    }
}
