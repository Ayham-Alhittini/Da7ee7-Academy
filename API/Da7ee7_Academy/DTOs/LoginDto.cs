using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "LoginProvider is required")]
        public string LoginProvider { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
