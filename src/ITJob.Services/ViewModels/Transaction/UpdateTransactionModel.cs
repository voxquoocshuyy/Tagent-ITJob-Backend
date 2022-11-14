namespace ITJob.Services.ViewModels.Transaction;

public class UpdateTransactionModel
{
    public Guid Id { get; set; }
    public double? Total { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? TypeOfTransaction { get; set; }
    public Guid? CreateBy { get; set; }
    public Guid? WalletId { get; set; }
}