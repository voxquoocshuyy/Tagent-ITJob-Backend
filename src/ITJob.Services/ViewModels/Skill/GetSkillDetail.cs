namespace ITJob.Services.ViewModels.Skill;

public class GetSkillDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? SkillGroupId { get; set; }
}