using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class ChangeProfileDto
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Gender { get; set; }
    }
}
