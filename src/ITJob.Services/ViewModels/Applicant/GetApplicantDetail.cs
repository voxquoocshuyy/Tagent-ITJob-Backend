namespace ITJob.Services.ViewModels.Applicant;

public class GetApplicantDetail
{
    public Guid Id { get; set; }
    public string Phone { get; set; } = null;
    public string Email { get; set; } = null;
    public string? Name { get; set; } = null!;
    public string? Avatar { get; set; } = null!;
    public int? Gender { get; set; } = null!;
    public DateTime? Dob { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public int? Status { get; set; } = null;
    public int? EarnMoney { get; set; } = null;
    public string? Reason { get; set; }
}