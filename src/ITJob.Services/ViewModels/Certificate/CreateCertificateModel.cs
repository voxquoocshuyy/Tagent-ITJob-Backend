namespace ITJob.Services.ViewModels.Certificate;

public class CreateCertificateModel
{
    public string Name { get; set; } = null!;
    public Guid? SkillGroupId { get; set; } = null!;
    public Guid? ProfileApplicantId { get; set; }
    public DateTime? GrantDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}