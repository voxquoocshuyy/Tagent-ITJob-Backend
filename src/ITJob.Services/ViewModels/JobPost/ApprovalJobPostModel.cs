namespace ITJob.Services.ViewModels.JobPost;

public class ApprovalJobPostModel
{
    public Guid Id { get; set; }
    public int Status { get; set; }
    public string? Reason { get; set; }
}