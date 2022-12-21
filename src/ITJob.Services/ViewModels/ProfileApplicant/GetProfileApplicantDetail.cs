using ITJob.Services.ViewModels.AlbumImage;
using ITJob.Services.ViewModels.Certificate;
using ITJob.Services.ViewModels.JobPosition;
using ITJob.Services.ViewModels.Like;
using ITJob.Services.ViewModels.ProfileApplicantSkill;
using ITJob.Services.ViewModels.Project;
using ITJob.Services.ViewModels.WorkingExperience;
using ITJob.Services.ViewModels.WorkingStyle;

namespace ITJob.Services.ViewModels.ProfileApplicant;

public class GetProfileApplicantDetail
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid? ApplicantId { get; set; }
    public string? Description { get; set; }
    public string? Education { get; set; }
    public string? GithubLink { get; set; }
    public string? LinkedInLink { get; set; }
    public string? FacebookLink { get; set; }
    public Guid? JobPositionId { get; set; }
    public Guid? WorkingStyleId { get; set; }
    public int? Status { get; set; }
    public int? CountLike { get; set; }
    // public virtual Entity.Entities.Applicant? Applicant { get; set; }
    public virtual GetJobPositionDetail JobPosition { get; set; }
    public virtual GetWorkingStyleDetail WorkingStyle { get; set; }
    public virtual ICollection<GetAlbumImageDetail> AlbumImages { get; set; }
    public virtual ICollection<GetCertificateDetail> Certificates { get; set; }
    // public virtual ICollection<GetLikeDetail> Likes { get; set; }
    public virtual ICollection<GetProfileApplicantSkillDetail> ProfileApplicantSkills { get; set; }
    public virtual ICollection<GetProjectDetail> Projects { get; set; }
    public virtual ICollection<GetWorkingExperienceDetail> WorkingExperiences { get; set; }
}