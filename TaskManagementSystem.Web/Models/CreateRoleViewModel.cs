using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Role name is required.")]
        public string Name { get; set; }

        public List<string> SelectedPermissions { get; set; } = new List<string>();

        public List<PermissionViewModel> Permissions { get; set; }
    }
}