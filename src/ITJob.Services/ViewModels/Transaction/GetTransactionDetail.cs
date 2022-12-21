using ITJob.Services.ViewModels.Product;

namespace ITJob.Services.ViewModels.Transaction;

public class GetTransactionDetail
{
    public Guid Id { get; set; }
    public double? Total { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? TypeOfTransaction { get; set; }
    public Guid? CreateBy { get; set; }
    public Guid? WalletId { get; set; }
    public Guid? ProductId { get; set; }
    public int? Quantity { get; set; }
    public virtual GetProductDetail Product { get; set; }
}