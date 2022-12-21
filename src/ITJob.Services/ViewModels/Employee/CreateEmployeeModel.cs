namespace ITJob.Services.ViewModels.Employee;

public class CreateEmployeeModel
{
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public Guid? CompanyId { get; set; }
}