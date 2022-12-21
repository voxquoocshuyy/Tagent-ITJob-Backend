using System.ComponentModel;

namespace ITJob.Services.ViewModels.Block;

public class SearchBlockModel
{
    [DefaultValue("")]
    public Guid? CompanyId { get; set; } = null;
    public Guid? ApplicantId { get; set; } = null;
    public Guid? BlockBy { get; set; } = null;
}