using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Like;

namespace ITJob.Services.Services.LikeServices;

public interface ILikeService
{
    IList<GetLikeDetail> GetLikePage(PagingParam<LikeEnum.LikeSort> paginationModel, SearchLikeModel searchLikeModel);

    public IList<GetLikeDetail> GetLikeDatePage(PagingParam<LikeEnum.LikeSort> paginationModel,
        SearchLikeModel searchLikeModel);

    public int GetLikeDateCompanyPage(PagingParam<LikeEnum.LikeSort> paginationModel, SearchLikeModel searchLikeModel);
    public Task<GetLikeDetail> GetLikeById(Guid id);
    public Task<GetLikeDetail> CreateLikeAsync(CreateLikeModel requestBody);
    public Task<GetLikeDetail> UpdateLikeAsync(Guid id, UpdateLikeModel requestBody);
    public Task<GetLikeDetail> CreateLikeForCompanyAsync(UpdateMatchModel requestBody);
    public Task<GetLikeDetail> CreateLikeForApplicantAsync(UpdateMatchModel requestBody);
    public Task DeleteLikeAsync(Guid id);
    public Task<int> GetTotal(); 
}