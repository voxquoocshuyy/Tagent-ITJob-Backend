namespace ITJob.Services.ViewModels.Wallet;

public class GetWalletDetail
{
    public Guid Id { get; set; }
    public double? Balance { get; set; }
    public int? Status { get; set; }
    public DateTime? CreateDate { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? ApplicantId { get; set; }
}