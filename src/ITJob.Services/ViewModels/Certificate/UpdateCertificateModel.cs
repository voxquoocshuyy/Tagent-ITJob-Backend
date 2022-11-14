namespace ITJob.Services.ViewModels.Certificate;

public class UpdateCertificateModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? SkillGroupId { get; set; }
    public Guid? ProfileApplicantId { get; set; }
    public DateTime? GrantDate { get; set; } = null;
    public DateTime? ExpiryDate { get; set; } = null;
}