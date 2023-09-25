namespace Da7ee7_Academy.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string TeacherId { get; set; }
        public string Major { get; set; }
        public string CoursePhotoUrl { get; set; }
        public bool isEnrolled { get; set; } = true;
        public List<SectionDto> Sections { get; set; }
    }
}
