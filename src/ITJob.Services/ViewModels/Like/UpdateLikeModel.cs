namespace ITJob.Services.ViewModels.Like;

public class UpdateLikeModel
{
    public Guid Id { get; set; }
    public Guid? JobPostId { get; set; }
    public Guid? ProfileApplicantId { get; set; }
    public bool? IsProfileApplicantLike { get; set; }
    public bool? IsJobPostLike { get; set; }
}