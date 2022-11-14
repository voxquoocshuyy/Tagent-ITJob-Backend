namespace ITJob.Services.ViewModels.JobPost;

public class JobPostDetailScore : GetJobPostDetail
{
    public float? Score { get; set; }
    public virtual ICollection<Entity.Entities.JobPostSkill> JobPostSkills { get; set; }
}