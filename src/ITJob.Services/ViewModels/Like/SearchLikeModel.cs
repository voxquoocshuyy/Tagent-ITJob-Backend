using System.ComponentModel;

namespace ITJob.Services.ViewModels.Like;

public class SearchLikeModel
{
    [DefaultValue("")]
    public Guid? JobPostId { get; set; } = null;
    public Guid? ProfileApplicantId { get; set; } = null;
    public int? IsProfileApplicantLike { get; set; } = null;
    public int? IsJobPostLike { get; set; } = null;
    public DateTime? CreateDate { get; set; } = null;
    public Guid? CompanyId { get; set; } = null;
    public DateTime? FromDate { get; set; } = null;
    public DateTime? ToDate { get; set; } = null;
    public int? Match { get; set; } = null;
}