namespace Paper_Route.Models.Documents
{

    public class DocumentRequestType // This is for the request that user is making, for example he needs a new birth certificate
    {
        public int Id { get; set; }                     // PK
        public string Name { get; set; } = string.Empty;   // e.g., "Birth Certificate"
        public string? Description { get; set; }        // Optional explanation for user
        public bool IsActive { get; set; } = true;      // Can deactivate old types

        public ICollection<DocumentCase> DocumentCases { get; set; } = new List<DocumentCase>();
    }

}
