using ITJob.Services.ViewModels.File;

namespace ITJob.Services.ViewModels.Applicant;

public class UpdateApplicantModel : FileViewModel
{
    public Guid Id { get; set; }
    public string Phone { get; set; } = null!;
    public string? Email { get; set; } = null;
    public string? Name { get; set; } = null;
    public int? Gender { get; set; } = null;
    public DateTime? Dob { get; set; } = null;
    public string? Address { get; set; } = null;
}