namespace ITJob.Services.ViewModels.User;

public class CreateUserModel
{
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid? RoleId { get; set; }
    public string? Password { get; set; }
}