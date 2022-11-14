using System.ComponentModel;

namespace ITJob.Services.ViewModels.Company;

public class SearchCompanyModel
{
    [DefaultValue("")]
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Logo { get; set; } = "";
    public string Website { get; set; } = "";
    public int? Status { get; set; } = null;
    public string? Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public int? CompanyType { get; set; } = null;
}