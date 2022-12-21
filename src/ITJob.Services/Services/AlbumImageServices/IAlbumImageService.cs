using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.AlbumImage;

namespace ITJob.Services.Services.AlbumImageServices;

public interface IAlbumImageService
{
    IList<GetAlbumImageDetail> GetAlbumImagePage(PagingParam<AlbumImageEnum.AlbumImageSort> paginationModel, SearchAlbumImageModel searchAlbumImageModel);

    public Task<GetAlbumImageDetail> GetAlbumImageById(Guid id);

    public Task<List<GetAlbumImageDetail>> CreateAlbumImageAsync(CreateAlbumImageModel requestBody);
    public Task<CreateAlbumImageUrlModel> CreateAlbumImageUrlAsync(CreateAlbumImageUrlModel requestBody);

    public Task<GetAlbumImageDetail> UpdateAlbumImageAsync(Guid id, UpdateAlbumImageModel requestBody);

    public Task DeleteAlbumImageAsync(Guid id);
    public Task DeleteAlbumImageByProfileApplicantIdAsync(Guid profileApplicantId);

    public Task<int> GetTotal();   
}