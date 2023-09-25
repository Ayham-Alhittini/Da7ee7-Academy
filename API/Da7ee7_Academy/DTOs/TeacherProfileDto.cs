namespace Da7ee7_Academy.DTOs
{
    public class TeacherProfileDto
    {
        public TeacherDto Teacher { get; set; }
        public IEnumerable<CourseDto> Courses { get; set; }
    }
}
