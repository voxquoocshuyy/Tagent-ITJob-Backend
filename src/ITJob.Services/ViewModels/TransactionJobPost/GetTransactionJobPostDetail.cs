namespace ITJob.Services.ViewModels.TransactionJobPost;

public class GetTransactionJobPostDetail
{
    public Guid Id { get; set; }
    public DateTime? Createdate { get; set; }
    public double? Total { get; set; }
    public int? Quantity { get; set; }
    public string? TypeOfTransaction { get; set; }
    public Guid? JobPostId { get; set; }
    public Guid? TransactionId { get; set; }
}