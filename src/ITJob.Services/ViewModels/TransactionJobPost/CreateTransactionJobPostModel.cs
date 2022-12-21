namespace ITJob.Services.ViewModels.TransactionJobPost;

public class CreateTransactionJobPostModel
{
    public Guid? JobPostId { get; set; }
    public Guid? CreateBy { get; set; }
    public Guid? Receiver { get; set; }
}