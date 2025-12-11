namespace AspTemp.Shared.Domain;

public class AuditableEntity
{
    public RecordStatus RecordStatus { get; set; } = RecordStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public bool IsActive => RecordStatus == RecordStatus.Active;
    public DateTime RecordStatusChangedAt { get; set; } = DateTime.UtcNow;
    public string RecordStatusChangedBy { get; set; } = string.Empty;
}

public enum RecordStatus
{
    Inactive = 1,
    Active = 2,
    Deleted = 3
}