using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class TeacherRegisterDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Major { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
