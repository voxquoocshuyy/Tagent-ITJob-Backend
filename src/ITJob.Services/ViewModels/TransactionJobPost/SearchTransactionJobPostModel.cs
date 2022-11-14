using System.ComponentModel;

namespace ITJob.Services.ViewModels.TransactionJobPost;

public class SearchTransactionJobPostModel
{
    [DefaultValue("")]
    public DateTime? Createdate { get; set; } = null;
    public string? TypeOfTransaction { get; set; } = "";
    public Guid? JobPostId { get; set; } = null;
    public Guid? TransactionId { get; set; } = null;
}