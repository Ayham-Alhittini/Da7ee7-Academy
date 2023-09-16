namespace Da7ee7_Academy.DTOs
{
    public class SectionItemDto
    {
        public int Id { get; set; }
        public string SectionItemTitle { get; set; }
        public int OrderNumber { get; set; }
        public int Type { get; set; }///Either attachment or lecture
        public string ContentUrl { get; set; }
        public int VideoLength { get; set; } //in seconds
        public string FileId { get; set; }
        public DateTime? WatchedDate { get; set; }//filled manual
    }
}
