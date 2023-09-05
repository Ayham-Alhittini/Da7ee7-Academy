namespace Da7ee7_Academy.Entities
{
    public class SectionItem
    {
        public int Id { get; set; }
        public string SectionItemTitle { get; set; }
        public int OrderNumber { get; set; }
        public int Type { get; set; }///Either attachment or lecture
        public string ContentUrl { get; set; }
        public string FileId { get; set; }
        public AppFile File { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
    }
}
