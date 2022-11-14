using ITJob.Services.ViewModels.File;

namespace ITJob.Services.ViewModels.AlbumImage;

public class UpdateAlbumImageModel : FileViewModel
{
    public Guid Id { get; set; }
    // public string UrlImage { get; set; } = null!;
    public Guid? ProfileApplicantId { get; set; }
    public Guid? JobPostId { get; set; }
    public Guid? ApplicantId { get; set; }
}