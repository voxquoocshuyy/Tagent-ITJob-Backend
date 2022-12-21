using System.ComponentModel;

namespace ITJob.Services.ViewModels.Applicant;

public class SearchApplicantModel
{
    [DefaultValue("")]
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Name { get; set; } = null;
    public string? Avatar { get; set; } = null;
    public int? Gender { get; set; } = null;
    public DateTime? Dob { get; set; } = null;
    public string? Address { get; set; } = null;
    public int? Status { get; set; } = null;
    public int? EarnMoney { get; set; } = null;
}