namespace ITJob.Services.ViewModels.TransactionJobPost;

public class GetTransactionJobPostDetail
{
    public Guid Id { get; set; }
    public DateTime? CreateDate { get; set; }
    public double? Total { get; set; }
    public int? Quantity { get; set; }
    public string? TypeOfTransaction { get; set; }
    public Guid? JobPostId { get; set; }
    public Guid? TransactionId { get; set; }
    public Guid? CreateBy { get; set; }
    public Guid? Receiver { get; set; }
    public Guid? MessageId { get; set; }
}