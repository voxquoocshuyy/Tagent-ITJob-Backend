using System.ComponentModel;

namespace ITJob.Services.ViewModels.SkillGroup;

public class SearchSkillGroupModel
{
    [DefaultValue("")]
    public string Name { get; set; } = "";
}