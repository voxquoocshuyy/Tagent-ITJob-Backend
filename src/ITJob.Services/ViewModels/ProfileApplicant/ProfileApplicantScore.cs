namespace ITJob.Services.ViewModels.ProfileApplicant;

public class ProfileApplicantScore : GetProfileApplicantDetail
{
    public float? Score { get; set; }
    public virtual ICollection<Entity.Entities.ProfileApplicantSkill> ProfileApplicantSkills { get; set; }
    public virtual Entity.Entities.Applicant? Applicant { get; set; }
}