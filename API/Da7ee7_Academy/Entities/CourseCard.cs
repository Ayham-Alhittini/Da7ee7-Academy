using System.ComponentModel.DataAnnotations;

namespace Da7ee7_Academy.Entities
{
    public class CourseCard
    {
        [Key]
        public string CardNumber { get; set; } = Guid.NewGuid().ToString();

        public string StudentId { get; set; }
        public Student Student { get; set; }

        ///Not set as a forigen key because intiality it would be empty while that not accepted 
        public int CourseId { get; set; }
    }
}
