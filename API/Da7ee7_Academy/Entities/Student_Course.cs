namespace Da7ee7_Academy.Entities
{
    public class Student_Course
    {
        public string StudentId { get; set; }
        public Student Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
