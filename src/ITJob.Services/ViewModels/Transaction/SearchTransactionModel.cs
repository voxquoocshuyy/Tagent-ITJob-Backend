using System.ComponentModel;

namespace ITJob.Services.ViewModels.Transaction;

public class SearchTransactionModel
{
    [DefaultValue("")]
    public DateTime? CreateDate { get; set; } = null;
    public string? TypeOfTransaction { get; set; } = "";
    public Guid? CreateBy { get; set; } = null;
    public Guid? WalletId { get; set; } = null;
}