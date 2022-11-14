namespace ITJob.Services.ViewModels.ProfileApplicantSkill;

public class GetProfileApplicantSkillDetail
{
    public Guid Id { get; set; }
    public Guid? ProfileApplicantId { get; set; }
    public Guid? SkillId { get; set; }
    public string? SkillLevel { get; set; }
}