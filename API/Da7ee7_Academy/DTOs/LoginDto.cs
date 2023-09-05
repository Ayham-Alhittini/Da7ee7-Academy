using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
