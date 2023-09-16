namespace Da7ee7_Academy.DTOs
{
    public class SectionDto
    {
        public int Id { get; set; }
        public string SectionTitle { get; set; }
        public int TotalSectionTime { get; set; } = 0;///stored in mintues , should be updated to long
        public int OrderNumber { get; set; }
        public int CourseId { get; set; }
        public List<SectionItemDto> SectionItems { get; set; }
    }
}
