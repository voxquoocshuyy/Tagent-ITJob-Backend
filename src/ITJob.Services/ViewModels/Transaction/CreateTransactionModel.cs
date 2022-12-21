using ITJob.Services.ViewModels.Product;

namespace ITJob.Services.ViewModels.Transaction;

public class CreateTransactionModel : TransactionProductModel
{
    public Guid? CreateBy { get; set; }
}