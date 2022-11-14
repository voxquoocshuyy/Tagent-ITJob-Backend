namespace ITJob.Services.ViewModels.ProfileApplicant;

public class CreateProfileApplicantModel
{
    public Guid ApplicantId { get; set; }
    public string? Description { get; set; }
    public string? Education { get; set; }
    public string? GithubLink { get; set; }
    public string? LinkedInLink { get; set; }
    public string? FacebookLink { get; set; }
    public Guid? JobPositionId { get; set; }
    public Guid? WorkingStyleId { get; set; }
    public int? Status { get; set; }
}