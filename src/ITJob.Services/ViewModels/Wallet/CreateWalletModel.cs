namespace ITJob.Services.ViewModels.Wallet;

public class CreateWalletModel
{
    public double? Balance { get; set; }
    public DateTime? CreateDate { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? ApplicantId { get; set; }
}