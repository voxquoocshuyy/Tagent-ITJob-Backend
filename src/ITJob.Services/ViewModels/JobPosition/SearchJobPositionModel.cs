using System.ComponentModel;

namespace ITJob.Services.ViewModels.JobPosition;

public class SearchJobPositionModel
{
    [DefaultValue("")]
    public string Name { get; set; } = "";
}