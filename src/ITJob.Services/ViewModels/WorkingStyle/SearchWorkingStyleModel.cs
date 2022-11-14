using System.ComponentModel;

namespace ITJob.Services.ViewModels.WorkingStyle;

public class SearchWorkingStyleModel
{
    [DefaultValue("")]
    public string Name { get; set; } = "";
}