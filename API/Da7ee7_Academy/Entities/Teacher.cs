using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Da7ee7_Academy.Entities
{
    public class Teacher
    {
        [Key, ForeignKey("AppUser")]
        public string Id { get; set; }
        public AppUser AppUser { get; set; }
        public string Major { get; set; }///the major here different that student, student could be ("علمي", "ادبي") teacher could be ("رياضيات", "انجليزي", "الخ")
        public string Gender { get; set; }
        public List<Course> Courses { get; set; }
        public List<Student_Course> TeachedStudents { get; set; }
    }
}