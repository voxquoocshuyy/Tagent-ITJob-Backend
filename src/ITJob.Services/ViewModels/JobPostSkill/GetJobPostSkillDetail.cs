namespace ITJob.Services.ViewModels.JobPostSkill;

public class GetJobPostSkillDetail
{
    public Guid Id { get; set; }
    public Guid? JobPostId { get; set; }
    public Guid? SkillId { get; set; }
    public string? SkillLevel { get; set; }
}