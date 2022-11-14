using System.ComponentModel;

namespace ITJob.Services.ViewModels.Certificate;

public class SearchCertificateModel
{
    [DefaultValue("")]
    public string Name { get; set; } = "";

    public Guid? SkillGroupId { get; set; } = null;
    public Guid? ProfileApplicantId { get; set; } = null;
    public DateTime? GrantDate { get; set; } = null;
    public DateTime? ExpiryDate { get; set; } = null;
}