using System.ComponentModel;

namespace ITJob.Services.ViewModels.ProfileApplicant;

public class SearchProfileApplicantModel
{
    [DefaultValue("")]
    public DateTime? CreateDate { get; set; } = null;
    public Guid? ApplicantId { get; set; } = null;
    public string Description { get; set; } = "";
    public string Education { get; set; } = "";
    public string GithubLink { get; set; } = "";
    public string LinkedInLink { get; set; } = "";
    public string FacebookLink { get; set; } = "";
    public Guid? JobPositionId { get; set; } = null;
    public Guid? WorkingStyleId { get; set; } = null;
    public int? Status { get; set; } = null;
}