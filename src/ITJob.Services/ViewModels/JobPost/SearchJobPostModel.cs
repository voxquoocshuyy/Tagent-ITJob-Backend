using System.ComponentModel;

namespace ITJob.Services.ViewModels.JobPost;

public class SearchJobPostModel
{
    [DefaultValue("")]
    public string Title { get; set; } = "";
    public DateTime? CreateDate { get; set; } = null;
    public string Description { get; set; } = "";
    public int? Status { get; set; } = null;
    public Guid? CompanyId { get; set; } = null;
    public Guid? JobPositionId { get; set; } = null;
    public Guid? EmployeeId { get; set; } = null;
    public Guid? WorkingStyleId { get; set; } = null;
    public string? WorkingPlace { get; set; } = "";
    public DateTime? StartTime { get; set; } = null;
    public DateTime? EndTime { get; set; } = null;
}