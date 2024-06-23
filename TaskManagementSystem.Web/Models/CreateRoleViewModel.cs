using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class CreateRoleViewModel
    {
        public string RoleName { get; set; }
        public List<PermissionViewModel> Permissions { get; set; }=new List<PermissionViewModel>();
    }
}