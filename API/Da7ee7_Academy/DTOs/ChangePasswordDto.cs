using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
    }
}
