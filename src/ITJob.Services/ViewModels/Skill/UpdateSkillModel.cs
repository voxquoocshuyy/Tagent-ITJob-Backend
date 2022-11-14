namespace ITJob.Services.ViewModels.Skill;

public class UpdateSkillModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? SkillGroupId { get; set; }
}