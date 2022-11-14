using System.ComponentModel;

namespace ITJob.Services.ViewModels.Project;

public class SearchProjectModel
{
    [DefaultValue("")]
    public string Name { get; set; } = "";
    public string Link { get; set; } = "";
    public string Description { get; set; }  = "";
    public DateTime? StartTime { get; set; } = null;
    public DateTime? EndTime { get; set; } = null;
    public Guid? ProfileApplicantId { get; set; } = null;
    public string Skill { get; set; }  = "";
    public string JobPosition { get; set; } = "";
}