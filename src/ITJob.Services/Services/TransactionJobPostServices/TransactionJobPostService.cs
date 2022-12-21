using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.JobPostRepositories;
using ITJob.Entity.Repositories.ProfileApplicantRepositories;
using ITJob.Entity.Repositories.TransactionJobPostRepositories;
using ITJob.Entity.Repositories.TransactionRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.TransactionJobPost;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ITJob.Services.Services.TransactionJobPostServices;

public class TransactionJobPostService : ITransactionJobPostService
{
    private readonly double _earnByShare;
    private readonly int _shareDailyLimit;
    private readonly ITransactionJobPostRepository _transactionJobPostRepository;
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IApplicantRepository _applicantRepository;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IProfileApplicantRepository _profileApplicantRepository;

    public TransactionJobPostService(ITransactionJobPostRepository transactionJobPostRepository, IMapper mapper,
        ITransactionRepository transactionRepository, IConfiguration config, IWalletRepository walletRepository, IApplicantRepository applicantRepository, IJobPostRepository jobPostRepository, IProfileApplicantRepository profileApplicantRepository)
    {
        _transactionJobPostRepository = transactionJobPostRepository;
        _mapper = mapper;
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _applicantRepository = applicantRepository;
        _jobPostRepository = jobPostRepository;
        _profileApplicantRepository = profileApplicantRepository;
        _earnByShare = double.Parse(config["SystemConfiguration:EarnByShare"]);
        _shareDailyLimit = int.Parse(config["SystemConfiguration:ShareDailyLimit"]);
    }
    public IList<GetTransactionJobPostDetail> GetTransactionJobPostPage(PagingParam<TransactionJobPostEnum.TransactionJobPostSort> paginationModel,
        SearchTransactionJobPostModel searchTransactionJobPostModel)
    {
        IQueryable<TransactionJobPost> queryTransactionJobPost = _transactionJobPostRepository.Table.Where(tjp => tjp.Total > 0);
        // queryTransactionJobPost = queryTransactionJobPost.GetWithSearch(searchTransactionJobPostModel);
        if (searchTransactionJobPostModel.FromDate != null && searchTransactionJobPostModel.ToDate != null)
        {
            queryTransactionJobPost = queryTransactionJobPost.Where(t => t.CreateDate >= searchTransactionJobPostModel.FromDate
                                                           && t.CreateDate <= searchTransactionJobPostModel.ToDate);  
        }
        if (searchTransactionJobPostModel.CreateBy != null)
        {
            queryTransactionJobPost = queryTransactionJobPost.Where(t => t.CreateBy == searchTransactionJobPostModel.CreateBy);
        }
        if (searchTransactionJobPostModel.TypeOfTransaction != null)
        {
            queryTransactionJobPost = queryTransactionJobPost.Where(t => t.TypeOfTransaction.Contains(searchTransactionJobPostModel.TypeOfTransaction));
        }
        if (searchTransactionJobPostModel.JobPostId != null)
        {
            queryTransactionJobPost = queryTransactionJobPost.Where(t => t.JobPostId == searchTransactionJobPostModel.JobPostId);
        }
        if (searchTransactionJobPostModel.TransactionId != null)
        {
            queryTransactionJobPost = queryTransactionJobPost.Where(t => t.TransactionId == searchTransactionJobPostModel.TransactionId);
        }
        // Apply sort
        queryTransactionJobPost = queryTransactionJobPost.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryTransactionJobPost = queryTransactionJobPost.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetTransactionJobPostDetail>(queryTransactionJobPost);
        return result.ToList();
    }

    public async Task<GetTransactionJobPostDetail> GetTransactionJobPostById(Guid id)
    {
        TransactionJobPost transactionJobPost = await _transactionJobPostRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (transactionJobPost == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetTransactionJobPostDetail>(transactionJobPost);
        return result;
    }

    public async Task<GetTransactionJobPostDetail> CreateTransactionJobPostAsync(CreateTransactionJobPostModel requestBody)
    {
        var transactionJobPost = _mapper.Map<TransactionJobPost>(requestBody);
        if (transactionJobPost == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }

        var transactionJobPostInDb = await _transactionJobPostRepository.GetFirstOrDefaultAsync(tjp =>
            tjp.CreateBy == requestBody.CreateBy && tjp.TypeOfTransaction == "Share job post" && tjp.Receiver == requestBody.Receiver
            && tjp.JobPostId == requestBody.JobPostId);
        if (transactionJobPostInDb != null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "This job post has been shared before");
        }

        var transaction = await _transactionRepository.GetFirstOrDefaultAsync(t => t.CreateBy == requestBody.JobPostId);
        var jobPost = await _jobPostRepository.GetFirstOrDefaultAsync(t => t.Id == requestBody.JobPostId);
        var applicant = await _applicantRepository.GetFirstOrDefaultAsync(a => a.Id == requestBody.CreateBy);
        var listProfileApplicant = await _profileApplicantRepository.Get(pa => pa.ApplicantId == applicant.Id).ToListAsync();
        var profileApplicant = await _profileApplicantRepository.GetFirstOrDefaultAsync(pa=>pa.ApplicantId == applicant.Id);
        var countShare = 0;
        foreach (var profileApplicantIn in listProfileApplicant)
        {
            var countShareOfApplicant = (int)profileApplicantIn.CountShare;
            countShare += countShareOfApplicant;
        }
        if (countShare < _shareDailyLimit)
        {
            if (applicant.EarnMoney == (int?)ApplicantEnum.ApplicantEarnMoney.Earn)
            {
                if (jobPost.Money < _earnByShare)
                {
                    throw new CException(StatusCodes.Status400BadRequest, "Money of job post not enough!!! ");
                }
                jobPost.Money -= _earnByShare;
                var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.ApplicantId == applicant.Id);
                wallet.Balance += _earnByShare;
                _walletRepository.Update(wallet);
                await _walletRepository.SaveChangesAsync();
        
                transactionJobPost.CreateDate = DateTime.Now;
                transactionJobPost.Quantity = 1;
                transactionJobPost.Total = _earnByShare;
                transactionJobPost.TypeOfTransaction = "Share job post";
                transactionJobPost.MessageId = Guid.NewGuid();
                transactionJobPost.TransactionId = transaction.Id;
        
                await _transactionJobPostRepository.InsertAsync(transactionJobPost);
                await _transactionJobPostRepository.SaveChangesAsync();
                //update count like
                profileApplicant.CountShare += 1;
                _profileApplicantRepository.Update(profileApplicant);
                await _profileApplicantRepository.SaveChangesAsync();
            }

            if (applicant.EarnMoney == (int?)ApplicantEnum.ApplicantEarnMoney.NotEarn ||
                applicant.EarnMoney == (int?)ApplicantEnum.ApplicantEarnMoney.Pending)
            {
                transactionJobPost.CreateDate = DateTime.Now;
                transactionJobPost.Quantity = 1;
                transactionJobPost.Total = 0;
                transactionJobPost.TypeOfTransaction = "Share job post";
                transactionJobPost.MessageId = Guid.NewGuid();
                transactionJobPost.TransactionId = transaction.Id;
        
                await _transactionJobPostRepository.InsertAsync(transactionJobPost);
                await _transactionJobPostRepository.SaveChangesAsync();
                //update count like
                profileApplicant.CountShare += 1;
                _profileApplicantRepository.Update(profileApplicant);
                await _profileApplicantRepository.SaveChangesAsync();
            }
        }
        else
        {
            throw new CException(StatusCodes.Status400BadRequest, "You've run out of shares. Please wait another 24 hours !!! ");
        }
        GetTransactionJobPostDetail transactionJobPostDetail = _mapper.Map<GetTransactionJobPostDetail>(transactionJobPost);
        return transactionJobPostDetail;
    }

    public async Task<GetTransactionJobPostDetail> UpdateTransactionJobPostAsync(Guid id, UpdateTransactionJobPostModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        TransactionJobPost transactionJobPost = await _transactionJobPostRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (transactionJobPost == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        transactionJobPost = _mapper.Map(requestBody, transactionJobPost);
        _transactionJobPostRepository.Update(transactionJobPost);
        await _transactionJobPostRepository.SaveChangesAsync();
        GetTransactionJobPostDetail transactionJobPostDetail = _mapper.Map<GetTransactionJobPostDetail>(transactionJobPost);
        return transactionJobPostDetail;
    }

    public async Task DeleteTransactionJobPostAsync(Guid id)
    {
        TransactionJobPost transactionJobPost = await _transactionJobPostRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (transactionJobPost == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _transactionJobPostRepository.Delete(transactionJobPost);
        await _transactionJobPostRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _transactionJobPostRepository.GetAll().CountAsync();
    }
}