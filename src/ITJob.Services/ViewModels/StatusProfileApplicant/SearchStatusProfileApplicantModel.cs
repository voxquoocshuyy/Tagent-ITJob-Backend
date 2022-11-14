using System.ComponentModel;

namespace ITJob.Services.ViewModels.StatusProfileApplicant;

public class SearchStatusProfileApplicantModel
{
    [DefaultValue("")]
    public string? Name { get; set; } = "";
    public Guid? ProfileApplicantId { get; set; } = null;
}