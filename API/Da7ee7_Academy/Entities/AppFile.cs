namespace Da7ee7_Academy.Entities
{
    public class AppFile
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();///the name of file to store in folders
        public string Path { get; set; }///in which folders this file stored
        public string ContentType { get; set; }///image video pdf ...
        public string FileName { get; set; }
    }
}
