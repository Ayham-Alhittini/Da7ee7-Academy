using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }


        [Compare("NewPassword", ErrorMessage = "Password & confirm password does not match")]
        public string ConfirmNewPassword { get; set; }
    }
}
