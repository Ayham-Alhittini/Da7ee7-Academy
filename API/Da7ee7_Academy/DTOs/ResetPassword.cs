using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password & confirm password does not match")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

    }
}
