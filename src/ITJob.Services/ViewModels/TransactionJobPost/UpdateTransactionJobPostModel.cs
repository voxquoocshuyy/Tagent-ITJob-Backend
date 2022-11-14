namespace ITJob.Services.ViewModels.TransactionJobPost;

public class UpdateTransactionJobPostModel
{
    public Guid Id { get; set; }
    public int? Quantity { get; set; }
    public double? Total { get; set; }
    public string? TypeOfTransaction { get; set; }
    public Guid? JobPostId { get; set; }
    public Guid? TransactionId { get; set; }
}