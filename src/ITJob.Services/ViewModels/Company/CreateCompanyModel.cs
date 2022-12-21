using ITJob.Services.ViewModels.File;
using Microsoft.AspNetCore.Http;

namespace ITJob.Services.ViewModels.Company;

public class CreateCompanyModel : FileViewModel
{
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Password { get; set; }
    public string Website { get; set; } = null!;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? CompanyType { get; set; }
    public string? TaxCode { get; set; }
}