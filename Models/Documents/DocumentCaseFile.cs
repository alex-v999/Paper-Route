namespace Paper_Route.Models.Documents
{
    public class DocumentCaseFile
    {
        public int Id { get; set; }
        public int DocumentCaseId { get; set; }
        public DocumentCase DocumentCase { get; set; } = null!;

        public int DocumentId { get; set; }                     // Link to profile document
        public Document Document { get; set; } = null!;

        public FileRole Role { get; set; } = FileRole.Input;
        public DateTime LinkedAt { get; set; } = DateTime.UtcNow;
    }

    public enum FileRole
    {
        Input = 0,
        Output = 1
    }

}
