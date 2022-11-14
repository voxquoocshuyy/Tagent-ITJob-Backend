namespace ITJob.Services.ViewModels.Skill;

public class CreateSkillModel
{
    public string Name { get; set; } = null!;
    public Guid? SkillGroupId { get; set; }
}