using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.TransactionJobPost;

namespace ITJob.Services.Services.TransactionJobPostServices;

public interface ITransactionJobPostService
{
    IList<GetTransactionJobPostDetail> GetTransactionJobPostPage(PagingParam<TransactionJobPostEnum.TransactionJobPostSort> paginationModel,
        SearchTransactionJobPostModel searchTransactionJobPostModel);

    public Task<GetTransactionJobPostDetail> GetTransactionJobPostById(Guid id);

    public Task<GetTransactionJobPostDetail> CreateTransactionJobPostAsync(CreateTransactionJobPostModel requestBody);

    public Task<GetTransactionJobPostDetail> UpdateTransactionJobPostAsync(Guid id, UpdateTransactionJobPostModel requestBody);

    public Task DeleteTransactionJobPostAsync(Guid id);

    public Task<int> GetTotal();
}