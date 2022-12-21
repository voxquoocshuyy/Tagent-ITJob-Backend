namespace ITJob.Services.ViewModels.AlbumImage;

public class CreateAlbumImageUrlModel
{
    public string UrlImage { get; set; } = null!;
    public Guid? JobPostId { get; set; }
}