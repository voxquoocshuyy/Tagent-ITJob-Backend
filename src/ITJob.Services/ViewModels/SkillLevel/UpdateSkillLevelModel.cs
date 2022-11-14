namespace ITJob.Services.ViewModels.SkillLevel;

public class UpdateSkillLevelModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? SkillGroupId { get; set; } = null;
}