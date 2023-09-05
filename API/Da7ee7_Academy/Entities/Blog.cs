namespace Da7ee7_Academy.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public AppUser Author { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
