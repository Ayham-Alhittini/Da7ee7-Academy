namespace Da7ee7_Academy.Entities
{
    public class WatchedLecture
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string StudentId { get; set; }
        public Student Student { get; set; }

        public int SectionItemId { get; set; }
        public SectionItem SectionItem { get; set; }

        public DateTime WatchedDate { get; set; } = DateTime.UtcNow;
    }
}

/*
 two choices
1- for each section item 
    search if this student does watch this lecture

2- get the watched lecture where course = req.course && user = req.user

put the result on hash map and check and result
 

#both work but i belive the second one is much better in terms of performance


one problem about the second one that it's not async if using any hash map the method should be blocked
so not wrong data filled with it 


#i could send the watched lecture for a course and student and he analyiz them in the angular application
so we don't worry about async stuff
 */