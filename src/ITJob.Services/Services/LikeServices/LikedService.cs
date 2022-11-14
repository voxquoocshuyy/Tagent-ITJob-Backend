using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.LikeRepositories;
using ITJob.Entity.Repositories.ProfileApplicantRepositories;
using ITJob.Entity.Repositories.TransactionJobPostRepositories;
using ITJob.Entity.Repositories.TransactionRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Like;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ITJob.Services.Services.LikeServices;

public class LikeService : ILikeService
{
    private readonly double _earnByLike;
    private readonly double _earnByMatch;
    private readonly ILikeRepository _likeRepository;
    private readonly IMapper _mapper;
    private readonly ITransactionJobPostRepository _transactionJobPostRepository;
    private readonly IApplicantRepository _applicantRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IProfileApplicantRepository _profileApplicantRepository;

    public LikeService(ILikeRepository likeRepository, IMapper mapper, ITransactionJobPostRepository transactionJobPostRepository,
        IApplicantRepository applicantRepository, IWalletRepository walletRepository, IConfiguration config,
        ITransactionRepository transactionRepository, IProfileApplicantRepository profileApplicantRepository)
    {
        _likeRepository = likeRepository;
        _mapper = mapper;
        _transactionJobPostRepository = transactionJobPostRepository;
        _applicantRepository = applicantRepository;
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _earnByLike = double.Parse(config["SystemConfiguration:EarnByLike"]);
        _earnByMatch = double.Parse(config["SystemConfiguration:EarnByMatch"]);
        _profileApplicantRepository = profileApplicantRepository;
    }
    public IList<GetLikeDetail> GetLikePage(PagingParam<LikeEnum.LikeSort> paginationModel, SearchLikeModel searchLikeModel)
    {
        IQueryable<Like> queryLike = _likeRepository.Table.Include(c => c.ProfileApplicant).Include(c => c.JobPost);
        queryLike = queryLike.GetWithSearch(searchLikeModel);
        // Apply sort
        queryLike = queryLike.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryLike = queryLike.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetLikeDetail>(queryLike);
        return result.ToList();
    }
    
    public async Task<GetLikeDetail> GetLikeById(Guid id)
    {
        var liked = await _likeRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (liked == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetLikeDetail>(liked);
        return result;
    }

    public async Task<GetLikeDetail> CreateLikeAsync(CreateLikeModel requestBody)
    {
        var liked = _mapper.Map<Like>(requestBody);
        if (liked == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        
        var profileApplicant =
            await _profileApplicantRepository.GetFirstOrDefaultAsync(pa => pa.Id == requestBody.ProfileApplicantId);
        var applicant =
            await _applicantRepository.GetFirstOrDefaultAsync(a => a.Id == profileApplicant.ApplicantId);
        
        if (requestBody.IsApplicantLike == 1)
        {
            if (applicant.IsEarningMoney == 1)
            {
                var transaction = 
                    await _transactionRepository.GetFirstOrDefaultAsync(t => t.CreateBy == requestBody.JobPostId);
                transaction.Total -= _earnByLike;
                if (transaction.Total < _earnByLike)
                {
                    throw new CException(StatusCodes.Status400BadRequest, "Money of job post not enough!!! ");
                }
                var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.ApplicantId == applicant.Id);
                wallet.Balance += _earnByLike;
                _walletRepository.Update(wallet);
                await _walletRepository.SaveChangesAsync();
                var transactionJobPost = new TransactionJobPost
                {
                    JobPostId = requestBody.JobPostId,
                    Quantity = 1,
                    TypeOfTransaction = "Like job post",
                    Total = _earnByLike,
                    TransactionId = transaction.Id
                };
                await _transactionJobPostRepository.InsertAsync(transactionJobPost);
                await _transactionJobPostRepository.SaveChangesAsync();
            }
        }
        await _likeRepository.InsertAsync(liked);
        await _likeRepository.SaveChangesAsync();
        //Send notification
        GetLikeDetail likedDetail = _mapper.Map<GetLikeDetail>(liked);
        return likedDetail;
    }
    public async Task<GetLikeDetail> CreateLikeForCompanyAsync(UpdateMatchModel requestBody)
    {
        var likedInDb = await _likeRepository.GetFirstOrDefaultAsync(l => l.JobPostId == requestBody.JobPostId &&
                                                                          l.ProfileApplicantId ==
                                                                          requestBody.ProfileApplicantId &&
                                                                          l.IsApplicantLike == 1);
        
        likedInDb = _mapper.Map(requestBody, likedInDb);
        likedInDb.Match = (int?)LikeEnum.Match.Match;
        _likeRepository.Update(likedInDb);
        await _likeRepository.SaveChangesAsync();
        
        var profileApplicant =
            await _profileApplicantRepository.GetFirstOrDefaultAsync(pa => pa.Id == requestBody.ProfileApplicantId);
        var applicant =
            await _applicantRepository.GetFirstOrDefaultAsync(a => a.Id == profileApplicant.ApplicantId);
        if (applicant.IsEarningMoney == 1)
        {
            var transaction = 
                await _transactionRepository.GetFirstOrDefaultAsync(t => t.CreateBy == requestBody.JobPostId);
            transaction.Total -= _earnByMatch;
            if (transaction.Total < _earnByMatch)
            {
                throw new CException(StatusCodes.Status400BadRequest, "Money of job post not enough!!! ");
            }
            var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.ApplicantId == applicant.Id);
            wallet.Balance += _earnByMatch;
            _walletRepository.Update(wallet);
            await _walletRepository.SaveChangesAsync();
            var transactionJobPost = new TransactionJobPost
            {
                JobPostId = requestBody.JobPostId,
                Quantity = 1,
                TypeOfTransaction = "Match",
                Total = _earnByMatch,
                TransactionId = transaction.Id
            };
            await _transactionJobPostRepository.InsertAsync(transactionJobPost);
            await _transactionJobPostRepository.SaveChangesAsync();
            //Send notification
        }
        GetLikeDetail likedDetail = _mapper.Map<GetLikeDetail>(likedInDb);
        return likedDetail;
    }
    public async Task<GetLikeDetail> CreateLikeForApplicantAsync(UpdateMatchModel requestBody)
    {
        Like likedInDb = await _likeRepository.GetFirstOrDefaultAsync(l => l.JobPostId == requestBody.JobPostId &&
                                                                           l.ProfileApplicantId == requestBody.ProfileApplicantId &&
                                                                           l.IsJobPostLike == 1);
        likedInDb = _mapper.Map(requestBody, likedInDb);
        likedInDb.Match = (int?)LikeEnum.Match.Match;
        _likeRepository.Update(likedInDb);
        await _likeRepository.SaveChangesAsync();
        
        var profileApplicant =
            await _profileApplicantRepository.GetFirstOrDefaultAsync(pa => pa.Id == requestBody.ProfileApplicantId);
        var applicant =
            await _applicantRepository.GetFirstOrDefaultAsync(a => a.Id == profileApplicant.ApplicantId);
        if (applicant.IsEarningMoney == 1)
        {
            var transaction =
                await _transactionRepository.GetFirstOrDefaultAsync(t => t.CreateBy == requestBody.JobPostId);
            transaction.Total -= _earnByMatch;
            if (transaction.Total < _earnByMatch)
            {
                throw new CException(StatusCodes.Status400BadRequest, "Money of job post not enough!!! ");
            }

            var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.ApplicantId == applicant.Id);
            wallet.Balance += _earnByMatch;
            _walletRepository.Update(wallet);
            await _walletRepository.SaveChangesAsync();
            var transactionJobPost = new TransactionJobPost
            {
                JobPostId = requestBody.JobPostId,
                Quantity = 1,
                TypeOfTransaction = "Match",
                Total = _earnByMatch,
                TransactionId = transaction.Id
            };
            await _transactionJobPostRepository.InsertAsync(transactionJobPost);
            await _transactionJobPostRepository.SaveChangesAsync();
            //Send notification
        }
        var likedDetail = _mapper.Map<GetLikeDetail>(likedInDb);
        return likedDetail;
    }
    public async Task<GetLikeDetail> UpdateLikeAsync(Guid id, UpdateLikeModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Like liked = await _likeRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (liked == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        liked = _mapper.Map(requestBody, liked);
        _likeRepository.Update(liked);
        await _likeRepository.SaveChangesAsync();
        var likedDetail = _mapper.Map<GetLikeDetail>(liked);
        return likedDetail;
    }

    public async Task DeleteLikeAsync(Guid id)
    {
        Like? liked = await _likeRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (liked == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _likeRepository.Delete(liked);
        await _likeRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _likeRepository.GetAll().CountAsync();
    }
}