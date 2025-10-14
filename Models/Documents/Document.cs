namespace Paper_Route.Models.Documents
{
    public class Document
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;

        public int DocumentTypeId { get; set; }
        public DocumentTypeEntity DocumentType { get; set; } = null!;

        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public bool IsVerified { get; set; } = false;
        public DateTime? ExpirationDate { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? VerifiedById { get; set; }
    }
}
