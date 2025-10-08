public class AvailableSlot
{
    public int Id { get; set; }

    public DateTime StartUtc { get; set; }

    public int DurationMinutes { get; set; }

    public SlotState State { get; set; }

    public int? WorkerProfileId { get; set; }

    public int? ConsultationId { get; set; }

    public string? Note { get; set; }
}

public enum SlotState { Available = 0, Booked = 1, Blocked = 2, Disabled = 3 }

public class SlotBlock  // to represent vacations, closures; used to generate Blocked slots or remove availability
{
    public int Id { get; set; }
    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public int? WorkerProfileId { get; set; } // null = global
    public string? Reason { get; set; }
}
