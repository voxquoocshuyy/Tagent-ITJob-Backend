namespace ITJob.Services.ViewModels.Block;

public class CreateBlockModel
{
    public Guid? CompanyId { get; set; }
    public Guid? ApplicantId { get; set; }
    public string? BlockBy { get; set; }
}