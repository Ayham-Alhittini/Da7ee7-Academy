using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Da7ee7_Academy.Entities
{
    public class Student
    {
        [Key, ForeignKey("AppUser")]
        public string Id { get; set; }
        public AppUser AppUser { get; set; }
        public string Major { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public List<Student_Course> Courses { get; set; }
    }
}
