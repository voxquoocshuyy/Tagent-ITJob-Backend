using System.ComponentModel;

namespace ITJob.Services.ViewModels.WorkingExperience;

public class SearchWorkingExperienceModel
{
    [DefaultValue("")]
    public Guid? ProfileApplicantId { get; set; }
    public string? CompanyName { get; set; }
    public Guid? JobPositionId { get; set; }
}