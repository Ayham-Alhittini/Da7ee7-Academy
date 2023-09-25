using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class CourseEnrollDto
    {
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string CardNumber { get; set; }
    }
}
