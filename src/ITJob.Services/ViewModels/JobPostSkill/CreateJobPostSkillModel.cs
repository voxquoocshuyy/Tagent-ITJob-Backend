namespace ITJob.Services.ViewModels.JobPostSkill;

public class CreateJobPostSkillModel
{
    public Guid? JobPostId { get; set; }
    public Guid? SkillId { get; set; }
    public string? SkillLevel { get; set; }
}