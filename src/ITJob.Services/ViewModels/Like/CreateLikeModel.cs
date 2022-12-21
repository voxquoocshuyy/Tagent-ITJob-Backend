namespace ITJob.Services.ViewModels.Like;

public class CreateLikeModel
{
    public Guid? JobPostId { get; set; }
    public Guid? ProfileApplicantId { get; set; }
    public bool? IsProfileApplicantLike { get; set; }
    public bool? IsJobPostLike { get; set; }
}