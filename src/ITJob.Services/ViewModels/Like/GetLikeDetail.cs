namespace ITJob.Services.ViewModels.Like;

public class GetLikeDetail
{
    public Guid Id { get; set; }
    public Guid? JobPostId { get; set; }
    public Guid? ProfileApplicantId { get; set; }
    public int? IsApplicantLike { get; set; }
    public int? IsJobPostLike { get; set; }
}