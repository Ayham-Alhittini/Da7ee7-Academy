namespace Da7ee7_Academy.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public string AppFileId { get; set; }
        public AppFile AppFile { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
