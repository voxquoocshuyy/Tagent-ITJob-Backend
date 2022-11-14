namespace ITJob.Services.ViewModels.WorkingExperience;

public class GetWorkingExperienceDetail
{
    public Guid Id { get; set; }
    public Guid? ProfileApplicantId { get; set; }
    public string? CompanyName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? JobPositionId { get; set; }
}