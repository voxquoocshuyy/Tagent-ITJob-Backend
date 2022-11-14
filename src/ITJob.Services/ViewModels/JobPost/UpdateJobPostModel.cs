namespace ITJob.Services.ViewModels.JobPost;

public class UpdateJobPostModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public string Description { get; set; } = null!;
    public int Quantity { get; set; }
    public int Status { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? JobPositionId { get; set; }
    public Guid? WorkingStyleId { get; set; }
    public string? WorkingPlace { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}