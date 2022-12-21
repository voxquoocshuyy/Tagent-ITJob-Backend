namespace ITJob.Services.ViewModels.User;

public class GetUserDetail
{
    public Guid Id { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int? Status { get; set; }
    public Guid? RoleId { get; set; }
    public string? Password { get; set; }
    public Guid? CompanyId { get; set; }
    public string? Reason { get; set; }
}