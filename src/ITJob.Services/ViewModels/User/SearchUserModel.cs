using System.ComponentModel;

namespace ITJob.Services.ViewModels.User;

public class SearchUserModel
{
    [DefaultValue("")]
    public string? Phone { get; set; } = "";
    public string? Email { get; set; } = "";
    public Guid? CompanyId { get; set; } = null;
    public int? Status { get; set; } = null;
}