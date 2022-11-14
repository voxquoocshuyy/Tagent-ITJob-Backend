namespace ITJob.Services.ViewModels.Certificate;

public class GetCertificateDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? SkillGroupId { get; set; } = null!;
    public Guid? ProfileApplicantId { get; set; }
    public DateTime? GrantDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}