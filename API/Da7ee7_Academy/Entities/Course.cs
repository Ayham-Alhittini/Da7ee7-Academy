namespace Da7ee7_Academy.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public string Major { get; set; }
        public string CoursePhotoUrl { get; set; }
        public string FileId { get; set; }
        public AppFile File { get; set; }
        public List<Student_Course> EnrolledStudents { get; set; }
        public List<Section> Sections { get; set; }
        public List<SectionItem> SectionItems { get; set; }/// <summary>
        /// will be used only to access the courseId from sectionItem 
        /// instead of fetch the section then get the course id 
        /// </summary>
        public List<WatchedLecture> WatchedLectures { get; set; }
    }
}
