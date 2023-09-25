using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.DTOs
{
    public class AddBlogDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public IFormFile BlogPhoto { get; set; }
        [Required]
        public string Content { get; set; }///description
    }
}
