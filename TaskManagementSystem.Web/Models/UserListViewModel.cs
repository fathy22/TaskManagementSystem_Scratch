using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Core.Entities;

namespace TaskManagementSystem.Web.Models
{
    public class UserListViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public string CurrentUserId { get; set; }
        public string AdminUserId { get; set; }
    }
}
