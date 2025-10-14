namespace Paper_Route.Models.Documents
{
    public class DocumentTypeEntity
    {
        public int Id { get; set; }                     // e.g. 1
        public string Name { get; set; } = string.Empty; // e.g. "Passport"
        public string? Description { get; set; }        // optional for admin UI
        public bool IsRequiredForVerification { get; set; } = false; // optional field
    }
}
