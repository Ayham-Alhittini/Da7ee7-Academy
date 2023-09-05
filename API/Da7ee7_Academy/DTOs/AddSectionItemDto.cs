using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class AddSectionItemDto
    {
        [Required]
        public int SectionId { get; set; }
        [Required]
        public string SectionItemTitle { get; set; }
        [Required]
        public int Type { get; set; }///Either attachment or lecture
        [Required]
        public IFormFile File { get; set; }
    }
}
