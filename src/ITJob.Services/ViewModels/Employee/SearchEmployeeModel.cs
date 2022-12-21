using System.ComponentModel;

namespace ITJob.Services.ViewModels.Employee;

public class SearchEmployeeModel
{
    [DefaultValue("")]
    public string? Phone { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? Name { get; set; } = null;
    public int? Status { get; set; } = null;
    public Guid? CompanyId { get; set; }  = null;
}