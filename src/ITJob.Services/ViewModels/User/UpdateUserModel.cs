namespace ITJob.Services.ViewModels.User;

public class UpdateUserModel
{
    public Guid Id { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int? Status { get; set; }
    public Guid? CompanyId { get; set; }
}