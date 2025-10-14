using System.Threading.Tasks;

namespace Paper_Route.Models.Documents
{
    public class DocumentCase // Basically A Request or case made by user to get something
    {
        public int Id { get; set; }  // <- primary key
        public string? Description { get; set; }

        public int RequestTypeId { get; set; }           // FK to predefined request types table
        public DocumentRequestType RequestType { get; set; } = null!; // Optional: hold enum or table

        public string UserId { get; set; }
        public string? WorkerId { get; set; }

        public CaseStatus Status { get; set; } = CaseStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<DocumentCaseFile> Files { get; set; } = new List<DocumentCaseFile>();
    }
    public enum CaseStatus
    {
        Pending,
        InProgress,
        Completed,
        Rejected
    }

}