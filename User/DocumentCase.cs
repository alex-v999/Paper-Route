using System.Threading.Tasks;

namespace Paper_Route.User
{
    public class DocumentCase
    {
        public int Id { get; set; }  // <- primary key
        public string Title { get; set; }
        public string UserId { get; set; }
        public string? WorkerId { get; set; }
        public CaseStatus Status { get; set; } = CaseStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
    public enum CaseStatus
    {
        Pending,
        InProgress,
        Completed,
        Rejected
    }
}