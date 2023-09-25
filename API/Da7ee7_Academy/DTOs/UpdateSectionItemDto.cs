using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class UpdateSectionItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string SectionItemTitle { get; set; }
        [Required]
        public int VideoLength { get; set; } //in seconds
        public IFormFile File { get; set; } 
    }
}
