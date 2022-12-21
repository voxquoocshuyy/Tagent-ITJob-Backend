using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.CompanyRepositories;
using ITJob.Entity.Repositories.JobPostRepositories;
using ITJob.Entity.Repositories.LikeRepositories;
using ITJob.Entity.Repositories.ProfileApplicantRepositories;
using ITJob.Entity.Repositories.TransactionJobPostRepositories;
using ITJob.Entity.Repositories.TransactionRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Services.Notification;
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
    private readonly int _likeDailyLimit;
    private readonly ILikeRepository _likeRepository;
    private readonly IMapper _mapper;
    private readonly ITransactionJobPostRepository _transactionJobPostRepository;
    private readonly IApplicantRepository _applicantRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IProfileApplicantRepository _profileApplicantRepository;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly ICompanyRepository _companyRepository;

    public LikeService(ILikeRepository likeRepository, IMapper mapper, ITransactionJobPostRepository transactionJobPostRepository,
        IApplicantRepository applicantRepository, IWalletRepository walletRepository, IConfiguration config,
        ITransactionRepository transactionRepository, IProfileApplicantRepository profileApplicantRepository, IJobPostRepository jobPostRepository, ICompanyRepository companyRepository)
    {
        _likeRepository = likeRepository;
        _mapper = mapper;
        _transactionJobPostRepository = transactionJobPostRepository;
        _applicantRepository = applicantRepository;
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _earnByLike = double.Parse(config["SystemConfiguration:EarnByLike"]);
        _earnByMatch = double.Parse(config["SystemConfiguration:EarnByMatch"]);
        _likeDailyLimit = int.Parse(config["SystemConfiguration:LikeDailyLimit"]);
        _profileApplicantRepository = profileApplicantRepository;
        _jobPostRepository = jobPostRepository;
        _companyRepository = companyRepository;
    }
    public IList<GetLikeDetail> GetLikePage(PagingParam<LikeEnum.LikeSort> paginationModel, SearchLikeModel searchLikeModel)
    {
        IQueryable<Like> queryLike = _likeRepository.Table.Include(c => c.ProfileApplicant)
            .Include(c => c.JobPost)
            .ThenInclude(jp => jp.JobPostSkills)
            .Include(c => c.JobPost)
            .ThenInclude(jp => jp.AlbumImages);
        queryLike = queryLike.GetWithSearch(searchLikeModel);
        // Apply sort
        queryLike = queryLike.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryLike = queryLike.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetLikeDetail>(queryLike);
        return result.ToList();
    }
    
    public IList<GetLikeDetail> GetLikeDatePage(PagingParam<LikeEnum.LikeSort> paginationModel, SearchLikeModel searchLikeModel)
    {
        IQueryable<Like> queryLike = _likeRepository.Table;
        if (searchLikeModel.FromDate != null && searchLikeModel.ToDate != null)
        {
            queryLike = queryLike.Where(t => t.CreateDate >= searchLikeModel.FromDate
                                             && t.CreateDate <= searchLikeModel.ToDate);  
        }
    
        if (searchLikeModel.Match == 1)
        {
            queryLike = queryLike.Where(t => t.Match == true); 
        }
    
        if (searchLikeModel.CompanyId != null)
        {
            var listJobPostIdOfCompany = _jobPostRepository
                .Get(jp => jp.CompanyId == searchLikeModel.CompanyId)
                .Select(jp => jp.Id ).ToList();
            var queryLike1 = _likeRepository.Table
                .Select(l => l.JobPostId).ToList();
            var listJobPostIdInLike = listJobPostIdOfCompany.Where(jp=>queryLike1.Contains(jp));
            var total = 0;
            foreach (var jobPostId in listJobPostIdInLike)
            {
                var queryLike2 = _likeRepository.Get(l => l.JobPostId == jobPostId);
                total += queryLike2.Count();
            }
        }
        var result = _mapper.ProjectTo<GetLikeDetail>(queryLike);
        return result.ToList();
    }
    public int GetLikeDateCompanyPage(PagingParam<LikeEnum.LikeSort> paginationModel, SearchLikeModel searchLikeModel)
    {
        var total = 0;
        if (searchLikeModel.CompanyId != null && searchLikeModel.FromDate != null && searchLikeModel.ToDate != null )
        {
            var listJobPostIdOfCompany = _jobPostRepository
                .Get(jp => jp.CompanyId == searchLikeModel.CompanyId)
                .Select(jp => jp.Id ).ToList();
            var queryLike1 = _likeRepository.Table.Where(t => t.CreateDate >= searchLikeModel.FromDate
                                                              && t.CreateDate <= searchLikeModel.ToDate)
                .Select(l => l.JobPostId).ToList();
            var listJobPostIdInLike = listJobPostIdOfCompany.Where(jp=>queryLike1.Contains(jp));
            foreach (var jobPostId in listJobPostIdInLike)
            {
                var queryLike2 = _likeRepository.Get(l => l.JobPostId == jobPostId);
                total += queryLike2.Count();
            }
        }
        return total;
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

        var jobPost = await _jobPostRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.JobPostId);
        var company = await _companyRepository.GetFirstOrDefaultAsync(c => c.Id == jobPost.CompanyId);

        var transaction =
            await _transactionRepository.GetFirstOrDefaultAsync(t => t.CreateBy == requestBody.JobPostId);

        if (requestBody.IsProfileApplicantLike == true)
        {
            var likedInDb = await _likeRepository.GetFirstOrDefaultAsync(l =>
                l.JobPostId == requestBody.JobPostId &&
                l.ProfileApplicantId == requestBody.ProfileApplicantId &&
                l.IsJobPostLike == true);
            
            //check like every day
            if (profileApplicant.CountLike < _likeDailyLimit)
            {
            
                if (likedInDb == null)
                {
                    if (applicant.EarnMoney == 1)
                    {
                        //check money in job post
                        if (jobPost.Money < _earnByLike)
                        {
                            throw new CException(StatusCodes.Status400BadRequest, "Money of job post not enough!!! ");
                        }

                        jobPost.Money -= _earnByLike;
                        //plus money to wallet
                        var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.ApplicantId == applicant.Id);
                        wallet.Balance += _earnByLike;
                        _walletRepository.Update(wallet);
                        await _walletRepository.SaveChangesAsync();
                        //create transaction
                        var transactionJobPost = new TransactionJobPost
                        {
                            JobPostId = requestBody.JobPostId,
                            Quantity = 1,
                            TypeOfTransaction = "Like job post",
                            Total = _earnByLike,
                            TransactionId = transaction.Id,
                            CreateBy = profileApplicant.Id
                        };
                        await _transactionJobPostRepository.InsertAsync(transactionJobPost);
                        await _transactionJobPostRepository.SaveChangesAsync();
                    }
                    //update count like
                    profileApplicant.CountLike += 1;
                    _profileApplicantRepository.Update(profileApplicant);
                    await _profileApplicantRepository.SaveChangesAsync();
                    // Sent notification
                    Dictionary<string, string> data = new Dictionary<string, string>()
                    {
                        { "type", "jobPost" },
                        { "jobPostId", jobPost.Id.ToString() }
                    };
                    await PushNotification.SendMessage(jobPost.EmployeeId.ToString(),
                        $"Bài tuyển dụng của bạn đã được ứng viên thích.",
                        $"Bài tuyển dụng {jobPost.Title} đã được ứng viên {applicant.Name} thích.", data);
                    //insert like
                    liked.IsJobPostLike = false;
                    await _likeRepository.InsertAsync(liked);
                    await _likeRepository.SaveChangesAsync();
                }
                
                if (likedInDb != null)
                {
                    //update like
                    likedInDb.Match = true;
                    likedInDb.MatchDate = DateTime.Now;

                    _likeRepository.Update(likedInDb);
                    await _likeRepository.SaveChangesAsync();

                    if (applicant.EarnMoney == 1)
                    {
                        //check money in job post
                        if (jobPost.Money < _earnByMatch)
                        {
                            throw new CException(StatusCodes.Status400BadRequest, "Money of job post not enough!!! ");
                        }

                        jobPost.Money -= _earnByMatch;
                        //plus money to wallet
                        var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.ApplicantId == applicant.Id);
                        wallet.Balance += _earnByMatch;
                        _walletRepository.Update(wallet);
                        await _walletRepository.SaveChangesAsync();
                        //create transaction
                        var transactionJobPost = new TransactionJobPost
                        {
                            JobPostId = requestBody.JobPostId,
                            Quantity = 1,
                            TypeOfTransaction = "Match",
                            Total = _earnByMatch,
                            TransactionId = transaction.Id,
                            CreateBy = profileApplicant.Id
                        };
                        await _transactionJobPostRepository.InsertAsync(transactionJobPost);
                        await _transactionJobPostRepository.SaveChangesAsync();
                        
                    }
                    //Send notification
                    Dictionary<string, string> data1 = new Dictionary<string, string>()
                    {
                        { "type", "profileApplicant" },
                        { "ApplicantId", applicant.Id.ToString() }
                    };
                    await PushNotification.SendMessage(profileApplicant.ApplicantId.ToString(),
                        $"Hồ sơ của bạn đã được kết nối.",
                        $"Hồ sơ của bạn đã được kết nối với một bài tuyển dụng của {company.Name}.", data1);
                    //Send notification
                    Dictionary<string, string> data2 = new Dictionary<string, string>()
                    {
                        { "type", "jobPost" },
                        { "jobPostId", jobPost.Id.ToString() }
                    };
                    await PushNotification.SendMessage(jobPost.EmployeeId.ToString(),
                        $"Bài tuyển dụng của bạn đã được kết nối.",
                        $"Bài tuyển dụng {jobPost.Title} của bạn đã được kết nối với một hồ sơ của {applicant.Name}.", data2);
                }
            }
            else
            {
                throw new CException(StatusCodes.Status400BadRequest,
                    "You've run out of likes. Please wait another 24 hours !!! ");
            }
        }
        
        if (requestBody.IsJobPostLike == true)
        {
            var likedInDb = await _likeRepository.GetFirstOrDefaultAsync(l => l.JobPostId == requestBody.JobPostId &&
                                                                              l.ProfileApplicantId ==
                                                                              requestBody.ProfileApplicantId &&
                                                                              l.IsProfileApplicantLike == true);
            if (likedInDb != null)
            {
                //update like
                likedInDb.Match = true;
                likedInDb.MatchDate = DateTime.Now;
                _likeRepository.Update(likedInDb);
                await _likeRepository.SaveChangesAsync();
                
                if (applicant.EarnMoney == 1)
                {
                    //check money in job post
                    if (jobPost.Money < _earnByMatch)
                    {
                        throw new CException(StatusCodes.Status400BadRequest, "Money of job post not enough!!! ");
                    }
                    jobPost.Money -= _earnByMatch;
                    //plus money to wallet
                    var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.ApplicantId == applicant.Id);
                    wallet.Balance += _earnByMatch;
                    _walletRepository.Update(wallet);
                    await _walletRepository.SaveChangesAsync();
                    //create transaction
                    var transactionJobPost = new TransactionJobPost
                    {
                        JobPostId = requestBody.JobPostId,
                        Quantity = 1,
                        TypeOfTransaction = "Match",
                        Total = _earnByMatch,
                        TransactionId = transaction.Id,
                        CreateBy = profileApplicant.Id
                    };
                    await _transactionJobPostRepository.InsertAsync(transactionJobPost);
                    await _transactionJobPostRepository.SaveChangesAsync();
                    
                }
                //Send notification
                Dictionary<string, string> data1 = new Dictionary<string, string>()
                {
                    { "type", "profileApplicant" },
                    { "profileApplicantId", profileApplicant.ApplicantId.ToString() }
                };
                await PushNotification.SendMessage(profileApplicant.ApplicantId.ToString(), $"Hồ sơ của bạn đã được kết nối.",
                    $"Hồ sơ của bạn đã được kết nối với một bài tuyển dụng của {company.Name}.", data1);
                //Send notification
                Dictionary<string, string> data2 = new Dictionary<string, string>()
                {
                    { "type", "jobPost" },
                    { "jobPostId", jobPost.Id.ToString() }
                };
                await PushNotification.SendMessage(jobPost.EmployeeId.ToString(),
                    $"Bài tuyển dụng của bạn đã được kết nối.",
                    $"Bài tuyển dụng {jobPost.Title} của bạn đã được kết nối với một hồ sơ của {applicant.Name}.", data2);
            }
            else if (likedInDb == null)
            {
                // Sent notification
                Dictionary<string, string> data = new Dictionary<string, string>()
                {
                    { "type", "profileApplicant" },
                    { "profileApplicantId", profileApplicant.Id.ToString() }
                };
                await PushNotification.SendMessage(profileApplicant.ApplicantId.ToString(), $"Hồ sơ của bạn đã được thích.",
                    $"Hồ sơ của bạn đã được thích bởi một bài viết của {company.Name}.", data);
                //insert like
                liked.IsProfileApplicantLike = false;
                await _likeRepository.InsertAsync(liked);
                await _likeRepository.SaveChangesAsync();
            }
        }
        GetLikeDetail likedDetail = _mapper.Map<GetLikeDetail>(liked);
        return likedDetail;
    }
    public async Task<GetLikeDetail> CreateLikeForCompanyAsync(UpdateMatchModel requestBody)
    {
        var likedInDb = await _likeRepository.GetFirstOrDefaultAsync(l => l.JobPostId == requestBody.JobPostId &&
                                                                          l.ProfileApplicantId ==
                                                                          requestBody.ProfileApplicantId &&
                                                                          l.IsProfileApplicantLike == true);
        
        likedInDb = _mapper.Map(requestBody, likedInDb);
        likedInDb.Match = true;
        likedInDb.MatchDate = DateTime.Now;
        _likeRepository.Update(likedInDb);
        await _likeRepository.SaveChangesAsync();
        
        var profileApplicant =
            await _profileApplicantRepository.GetFirstOrDefaultAsync(pa => pa.Id == requestBody.ProfileApplicantId);
        var applicant =
            await _applicantRepository.GetFirstOrDefaultAsync(a => a.Id == profileApplicant.ApplicantId);
        
        var jobPost = await _jobPostRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.JobPostId);
        var company = await _companyRepository.GetFirstOrDefaultAsync(c => c.Id == jobPost.CompanyId);
        
        var transaction = 
            await _transactionRepository.GetFirstOrDefaultAsync(t => t.CreateBy == requestBody.JobPostId);
        
        if (applicant.EarnMoney == 1)
        {
            jobPost.Money -= _earnByMatch;
            if (jobPost.Money < _earnByMatch)
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
                TransactionId = transaction.Id,
                CreateBy = profileApplicant.Id
            };
            await _transactionJobPostRepository.InsertAsync(transactionJobPost);
            await _transactionJobPostRepository.SaveChangesAsync();
            //Send notification
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "profileApplicant" },
                { "profileApplicantId", profileApplicant.Id.ToString() }
            };
            await PushNotification.SendMessage(profileApplicant.ApplicantId.ToString(), $"Hồ sơ của bạn đã được kết nối.",
                $"Hồ sơ của bạn đã được kết nối với một bài viết của {company.Name}.", data);
        }
        GetLikeDetail likedDetail = _mapper.Map<GetLikeDetail>(likedInDb);
        return likedDetail;
    }
    public async Task<GetLikeDetail> CreateLikeForApplicantAsync(UpdateMatchModel requestBody)
    {
        Like likedInDb = await _likeRepository.GetFirstOrDefaultAsync(l => l.JobPostId == requestBody.JobPostId &&
                                                                           l.ProfileApplicantId == requestBody.ProfileApplicantId &&
                                                                           l.IsJobPostLike == true);
        likedInDb = _mapper.Map(requestBody, likedInDb);
        likedInDb.Match = true;
        likedInDb.MatchDate = DateTime.Now;
        _likeRepository.Update(likedInDb);
        await _likeRepository.SaveChangesAsync();
        
        var profileApplicant =
            await _profileApplicantRepository.GetFirstOrDefaultAsync(pa => pa.Id == requestBody.ProfileApplicantId);
        var applicant =
            await _applicantRepository.GetFirstOrDefaultAsync(a => a.Id == profileApplicant.ApplicantId);
        
        var jobPost = await _jobPostRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.JobPostId);
        
        var transaction =
            await _transactionRepository.GetFirstOrDefaultAsync(t => t.CreateBy == requestBody.JobPostId);

        if (applicant.EarnMoney == 1)
        {
            jobPost.Money -= _earnByMatch;
            if (jobPost.Money < _earnByMatch)
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
                TransactionId = transaction.Id,
                CreateBy = profileApplicant.Id
            };
            await _transactionJobPostRepository.InsertAsync(transactionJobPost);
            await _transactionJobPostRepository.SaveChangesAsync();
            //Send notification
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "jobPost" },
                { "jobPostId", jobPost.Id.ToString() }
            };
            await PushNotification.SendMessage(jobPost.CompanyId.ToString(), $"Bài đăng của bạn đã được kết nối.",
                $"Bài đăng của bạn đã được kết nối với một hồ sơ của {applicant.Name}.", data);
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