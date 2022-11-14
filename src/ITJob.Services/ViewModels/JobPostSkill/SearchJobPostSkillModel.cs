using System.ComponentModel;

namespace ITJob.Services.ViewModels.JobPostSkill;

public class SearchJobPostSkillModel
{
    [DefaultValue("")]
    public Guid? JobPostId { get; set; } = null;
    public Guid? SkillId { get; set; } = null;
    public string? SkillLevel { get; set; } = "";
}