namespace ITJob.Services.ViewModels.Transaction;

public class CreateTransactionModel
{
    public double? Total { get; set; }
    public string? TypeOfTransaction { get; set; }
    public Guid? CreateBy { get; set; }
    public Guid? WalletId { get; set; }
}