using System.ComponentModel;

namespace ITJob.Services.ViewModels.Skill;

public class SearchSkillModel
{
    [DefaultValue("")]
    public string Name { get; set; } = "";
    public Guid? SkillGroupId { get; set; } = null;
}