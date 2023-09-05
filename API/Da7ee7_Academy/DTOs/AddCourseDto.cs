namespace Da7ee7_Academy.DTOs
{
    public class AddCourseDto
    {
        public string Subject { get; set; }
        public string TeacherId { get; set; }
        public string Major { get; set; }
        public IFormFile CourseCover { get; set; }
    }
}
