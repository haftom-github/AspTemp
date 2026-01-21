namespace AspTemp.Shared.Domain;

public interface IAuditable
{
    RecordStatus RecordStatus { get; set; }

    DateTime CreatedDate { get; set; }
    Guid? CreatedBy { get; set; }

    DateTime? UpdatedDate { get; set; }
    Guid? UpdatedBy { get; set; }
}
