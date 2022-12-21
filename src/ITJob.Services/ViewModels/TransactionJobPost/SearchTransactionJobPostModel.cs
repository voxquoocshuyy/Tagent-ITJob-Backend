using System.ComponentModel;

namespace ITJob.Services.ViewModels.TransactionJobPost;

public class SearchTransactionJobPostModel
{
    [DefaultValue("")]
    public string? TypeOfTransaction { get; set; } = "";
    public Guid? JobPostId { get; set; } = null;
    public Guid? CreateBy { get; set; } = null;
    public DateTime? FromDate { get; set; } = null;
    public DateTime? ToDate { get; set; } = null;
    public Guid? TransactionId { get; set; }
}