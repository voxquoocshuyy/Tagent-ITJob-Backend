namespace ITJob.Services.ViewModels.SkillLevel;

public class CreateSkillLevelModel
{
    public string Name { get; set; } = null!;
    public Guid? SkillGroupId { get; set; }
}