using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.TransactionJobPostRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.TransactionJobPost;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.TransactionJobPostServices;

public class TransactionJobPostService : ITransactionJobPostService
{
    private readonly ITransactionJobPostRepository _transactionJobPostRepository;
    private readonly IMapper _mapper;

    public TransactionJobPostService(ITransactionJobPostRepository transactionJobPostRepository, IMapper mapper)
    {
        _transactionJobPostRepository = transactionJobPostRepository;
        _mapper = mapper;
    }
    public IList<GetTransactionJobPostDetail> GetTransactionJobPostPage(PagingParam<TransactionJobPostEnum.TransactionJobPostSort> paginationModel, SearchTransactionJobPostModel searchTransactionJobPostModel)
    {
        IQueryable<TransactionJobPost> queryTransactionJobPost = _transactionJobPostRepository.Table;
        queryTransactionJobPost = queryTransactionJobPost.GetWithSearch(searchTransactionJobPostModel);
        // Apply sort
        queryTransactionJobPost = queryTransactionJobPost.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryTransactionJobPost = queryTransactionJobPost.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetTransactionJobPostDetail>(queryTransactionJobPost);
        return result.ToList();
    }

    public async Task<GetTransactionJobPostDetail> GetTransactionJobPostById(Guid id)
    {
        TransactionJobPost workingStyle = await _transactionJobPostRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetTransactionJobPostDetail>(workingStyle);
        return result;
    }

    public async Task<GetTransactionJobPostDetail> CreateTransactionJobPostAsync(CreateTransactionJobPostModel requestBody)
    {
        TransactionJobPost workingStyle = _mapper.Map<TransactionJobPost>(requestBody);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _transactionJobPostRepository.InsertAsync(workingStyle);
        await _transactionJobPostRepository.SaveChangesAsync();
        GetTransactionJobPostDetail workingStyleDetail = _mapper.Map<GetTransactionJobPostDetail>(workingStyle);
        return workingStyleDetail;
    }

    public async Task<GetTransactionJobPostDetail> UpdateTransactionJobPostAsync(Guid id, UpdateTransactionJobPostModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        TransactionJobPost workingStyle = await _transactionJobPostRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        workingStyle = _mapper.Map(requestBody, workingStyle);
        _transactionJobPostRepository.Update(workingStyle);
        await _transactionJobPostRepository.SaveChangesAsync();
        GetTransactionJobPostDetail workingStyleDetail = _mapper.Map<GetTransactionJobPostDetail>(workingStyle);
        return workingStyleDetail;
    }

    public async Task DeleteTransactionJobPostAsync(Guid id)
    {
        TransactionJobPost workingStyle = await _transactionJobPostRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _transactionJobPostRepository.Delete(workingStyle);
        await _transactionJobPostRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _transactionJobPostRepository.GetAll().CountAsync();
    }
}