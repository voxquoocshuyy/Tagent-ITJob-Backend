using ITJob.Services.ViewModels.AlbumImage;
using ITJob.Services.ViewModels.Certificate;
using ITJob.Services.ViewModels.JobPost;
using ITJob.Services.ViewModels.ProfileApplicant;
using ITJob.Services.ViewModels.ProfileApplicantSkill;
using ITJob.Services.ViewModels.Project;
using ITJob.Services.ViewModels.WorkingExperience;

namespace ITJob.Services.ViewModels.Like;

public class GetLikeDetail
{
    public Guid Id { get; set; }
    public Guid? JobPostId { get; set; }
    public Guid? ProfileApplicantId { get; set; }
    public bool? IsProfileApplicantLike { get; set; }
    public bool? IsJobPostLike { get; set; }
    public bool? Match { get; set; }
    public DateTime? MatchDate { get; set; }
    public virtual GetJobPostDetail JobPost { get; set; }
    public virtual GetProfileApplicantDetail ProfileApplicant { get; set; }
}