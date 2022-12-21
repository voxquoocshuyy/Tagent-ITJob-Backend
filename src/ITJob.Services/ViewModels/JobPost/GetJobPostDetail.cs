using ITJob.Services.ViewModels.AlbumImage;
using ITJob.Services.ViewModels.JobPosition;
using ITJob.Services.ViewModels.JobPostSkill;
using ITJob.Services.ViewModels.Like;
using ITJob.Services.ViewModels.WorkingStyle;

namespace ITJob.Services.ViewModels.JobPost;

public class GetJobPostDetail
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public string Description { get; set; } = null!;
    public int Quantity { get; set; }
    public int Status { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? JobPositionId { get; set; }
    public Guid? WorkingStyleId { get; set; }
    public string? WorkingPlace { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Reason { get; set; }
    public DateTime? ApproveDate { get; set; }
    public double? Money { get; set; }
    public Guid? EmployeeId { get; set; }
    // public virtual Entity.Entities.Company? Company { get; set; }
    public virtual GetJobPositionDetail JobPosition { get; set; }
    public virtual GetWorkingStyleDetail WorkingStyle { get; set; }
    public virtual ICollection<GetAlbumImageDetail> AlbumImages { get; set; }
    public virtual ICollection<GetJobPostSkillDetail> JobPostSkills { get; set; }
    // public virtual ICollection<GetLikeDetail> Likes { get; set; }
}