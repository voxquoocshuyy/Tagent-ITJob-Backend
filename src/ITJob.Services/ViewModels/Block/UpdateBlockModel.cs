namespace ITJob.Services.ViewModels.Block;

public class UpdateBlockModel
{
    public Guid Id { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? ApplicantId { get; set; }
    public Guid? BlockBy { get; set; }
}