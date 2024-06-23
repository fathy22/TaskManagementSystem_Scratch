using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class PermissionViewModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}