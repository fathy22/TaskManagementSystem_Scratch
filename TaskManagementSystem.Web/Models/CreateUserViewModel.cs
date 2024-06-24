using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models
{
    public class CreateUserViewModel
    {

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

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "At least one role must be selected.")]
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}
