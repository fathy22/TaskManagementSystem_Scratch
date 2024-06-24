using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Second Name is required.")]
        [StringLength(50, ErrorMessage = "Second Name cannot exceed 50 characters.")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "At least one role must be selected.")]
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}
