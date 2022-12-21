namespace ITJob.Services.ViewModels.Employee;

public class GetEmployeeDetail
{
    public Guid Id { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public int? Status { get; set; }
    public string? Reason { get; set; }
    public Guid? CompanyId { get; set; }
}