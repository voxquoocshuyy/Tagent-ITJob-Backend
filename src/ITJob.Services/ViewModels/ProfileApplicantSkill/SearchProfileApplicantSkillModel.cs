using System.ComponentModel;

namespace ITJob.Services.ViewModels.ProfileApplicantSkill;

public class SearchProfileApplicantSkillModel
{
    [DefaultValue("")] 
    public Guid? ProfileApplicantId { get; set; } = null;
    public Guid? SkillId { get; set; } = null;
    public string? SkillLevel { get; set; } = "";
}