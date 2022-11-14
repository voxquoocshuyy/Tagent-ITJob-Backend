namespace ITJob.Services.ViewModels.Like;

public class CreateLikeModel
{
    public Guid? JobPostId { get; set; }
    public Guid? ProfileApplicantId { get; set; }
    public int? IsApplicantLike { get; set; }
    public int? IsJobPostLike { get; set; }
}