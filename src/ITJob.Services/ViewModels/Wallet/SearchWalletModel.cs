using System.ComponentModel;

namespace ITJob.Services.ViewModels.Wallet;

public class SearchWalletModel
{
    [DefaultValue("")]
    public int? Status { get; set; } = null;
    public DateTime? CreateDate { get; set; } = null;
    public Guid? CompanyId { get; set; } = null;
    public Guid? ApplicantId { get; set; } = null;
}