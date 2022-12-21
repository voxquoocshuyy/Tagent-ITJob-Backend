using System.ComponentModel;

namespace ITJob.Services.ViewModels.Transaction;

public class SearchTransactionModel
{
    [DefaultValue("")]
    public string? TypeOfTransaction { get; set; } = "";
    public Guid? CreateBy { get; set; } = null;
    public DateTime? FromDate { get; set; } = null;
    public DateTime? ToDate { get; set; } = null;
    public Guid? WalletId { get; set; } = null;
}