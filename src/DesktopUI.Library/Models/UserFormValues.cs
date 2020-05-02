using System.ComponentModel.DataAnnotations;

namespace DesktopUI.Library.Models
{
    public class UserFormValues
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Must not be empty.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Must not be empty.")]
        public string Password { get; set; }
    }
}