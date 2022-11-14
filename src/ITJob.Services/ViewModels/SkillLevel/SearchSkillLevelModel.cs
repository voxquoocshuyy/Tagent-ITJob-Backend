using System.ComponentModel;

namespace ITJob.Services.ViewModels.SkillLevel;

public class SearchSkillLevelModel
{
    [DefaultValue("")]
    public string Name { get; set; } = "";

    public Guid? SkillGroupId { get; set; } = null;
}