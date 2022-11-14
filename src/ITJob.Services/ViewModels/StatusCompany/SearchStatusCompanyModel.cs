using System.ComponentModel;

namespace ITJob.Services.ViewModels.StatusCompany;

public class SearchStatusCompanyModel
{
    [DefaultValue("")]
    public string? Name { get; set; } = "";
    public Guid? CompanyId { get; set; } = null;
}