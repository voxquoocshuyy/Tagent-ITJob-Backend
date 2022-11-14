using System.ComponentModel;

namespace ITJob.Services.ViewModels.Role;

public class SearchRoleModel
{
    [DefaultValue("")]
    public string Name { get; set; } = "";
}