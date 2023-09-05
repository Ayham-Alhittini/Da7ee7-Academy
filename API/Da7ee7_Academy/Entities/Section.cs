namespace Da7ee7_Academy.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public string SectionTitle { get; set; }
        public int TotalSectionTime { get; set; } = 0;///could be in hours or minutes
        public int OrderNumber { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<SectionItem> SectionItems { get; set; }
    }
}
