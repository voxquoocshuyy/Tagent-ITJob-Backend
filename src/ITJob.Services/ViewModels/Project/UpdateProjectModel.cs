namespace ITJob.Services.ViewModels.Project;

public class UpdateProjectModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Link { get; set; }
    public string? Description { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Skill { get; set; }
    public string? JobPosition { get; set; }
    public Guid? ProfileApplicantId { get; set; }
}