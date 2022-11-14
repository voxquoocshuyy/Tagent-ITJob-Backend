using System.ComponentModel;

namespace ITJob.Services.ViewModels.AlbumImage;

public class SearchAlbumImageModel
{
    [DefaultValue("")]
    public string UrlImage { get; set; } = "";
    public Guid? ProfileApplicantId { get; set; } = null;
    public Guid? JobPostId { get; set; } = null;
    public Guid? ApplicantId { get; set; } = null;
    public DateTime? CreateDate { get; set; } = null;
}