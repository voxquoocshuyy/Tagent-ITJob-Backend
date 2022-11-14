namespace ITJob.Services.ViewModels.ProfileApplicantSkill;

public class CreateProfileApplicantSkillModel
{
    public Guid? ProfileApplicantId { get; set; }
    public Guid? SkillId { get; set; }
    public string? SkillLevel { get; set; }
}