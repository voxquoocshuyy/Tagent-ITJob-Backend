using System.ComponentModel;

namespace ITJob.Services.ViewModels.Like;

public class SearchLikeModel
{
    [DefaultValue("")]
    public Guid? JobPostId { get; set; } = null;
    public Guid? ProfileApplicantId { get; set; } = null;
    public int? IsApplicantLike { get; set; } = null;
    public int? IsJobPostLike { get; set; } = null;
    public DateTime? CreateDate { get; set; } = null;
    public int? Match { get; set; } = null;
}