using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        public string Name { get; set; }

        public List<string> SelectedPermissions { get; set; } = new List<string>();

        public List<PermissionViewModel> Permissions { get; set; } = new List<PermissionViewModel>();
    }
}