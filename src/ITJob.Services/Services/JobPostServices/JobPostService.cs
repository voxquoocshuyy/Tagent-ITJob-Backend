using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.JobPostRepositories;
using ITJob.Entity.Repositories.LikeRepositories;
using ITJob.Entity.Repositories.SystemWalletRepositories;
using ITJob.Entity.Repositories.TransactionRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.JobPost;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ITJob.Services.Services.JobPostServices;

public class JobPostService : IJobPostService
{
    private readonly double _exchangeRate;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly ISystemWalletRepository _systemWalletRepository;

    public JobPostService(IJobPostRepository jobPostRepository, IMapper mapper, ITransactionRepository
        transactionRepository, IWalletRepository walletRepository, IConfiguration config, ILikeRepository likeRepository,
        ISystemWalletRepository systemWalletRepository)
    {
        _jobPostRepository = jobPostRepository;
        _mapper = mapper;
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _exchangeRate = double.Parse(config["SystemConfiguration:ExchangeRate"]);
        _likeRepository = likeRepository;
        _systemWalletRepository = systemWalletRepository;
    }
    public IList<GetJobPostDetail> GetJobPostPage(PagingParam<JobPostEnum.JobPostSort> paginationModel, SearchJobPostModel searchJobPostModel)
    {
        IQueryable<JobPost> queryJobPost = _jobPostRepository.Table.Include(c => c.Company);
        queryJobPost = queryJobPost.GetWithSearch(searchJobPostModel);
        // Apply sort
        queryJobPost = queryJobPost.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryJobPost = queryJobPost.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetJobPostDetail>(queryJobPost);
        return result.ToList();
    }
    public IList<GetJobPostDetail> GetJobPostLikePage(
        PagingParam<JobPostEnum.JobPostSort> paginationModel, 
        SearchJobPostModel searchProfileApplicantModel, Guid profileApplicantId)
    {
        IQueryable<JobPost?> queryJobPost =
            _likeRepository.Get(l => l.ProfileApplicantId == profileApplicantId)
                .Where(l => l.IsJobPostLike == 1 && l.Match == null)
                .Include(p =>
                p.JobPost).Select(p => p.JobPost);
        queryJobPost = queryJobPost.GetWithSearch(searchProfileApplicantModel);
        // Apply sort
        queryJobPost = queryJobPost.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryJobPost = queryJobPost.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetJobPostDetail>(queryJobPost);
        return result.ToList();
    }
    public async Task<GetJobPostDetail> GetJobPostById(Guid id)
    {
        JobPost jobPost = await _jobPostRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (jobPost == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetJobPostDetail>(jobPost);
        return result;
    }

    public async Task<GetJobPostDetail> CreateJobPostAsync(CreateJobPostModel requestBody)
    {
        JobPost jobPost = _mapper.Map<JobPost>(requestBody);
        if (jobPost == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        jobPost.Status = (int)JobPostEnum.JobPostStatus.Pending;
        jobPost.Money = requestBody.MoneyForJobPost / _exchangeRate;
        await _jobPostRepository.InsertAsync(jobPost);
        await _jobPostRepository.SaveChangesAsync();
        var jobPostDetail = _mapper.Map<GetJobPostDetail>(jobPost);
        return jobPostDetail;
    }

    public async Task<GetJobPostDetail> UpdateJobPostAsync(Guid id, UpdateJobPostModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var jobPost = await _jobPostRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (jobPost == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        jobPost = _mapper.Map(requestBody, jobPost);
        _jobPostRepository.Update(jobPost);
        await _jobPostRepository.SaveChangesAsync();
        GetJobPostDetail jobPostDetail = _mapper.Map<GetJobPostDetail>(jobPost);
        return jobPostDetail;
    }
    public async Task<GetJobPostDetail> ApprovalJobPostAsync(Guid id, ApprovalJobPostModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var jobPost = await _jobPostRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (jobPost == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var moneyOfJobPost = jobPost.Money;
        var moneyForJobPost = (moneyOfJobPost * 60) / 100;
        var moneyForSystem = moneyOfJobPost - moneyForJobPost;
        if (requestBody.Status == (int)JobPostEnum.JobPostStatus.Posting)
        {
            jobPost.ApproveDate = DateTime.Now;
            jobPost.Status = requestBody.Status;
            _jobPostRepository.Update(jobPost);
            await _jobPostRepository.SaveChangesAsync();
            var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.CompanyId == jobPost.CompanyId);
            var balance = wallet.Balance;
            wallet.Balance = balance - moneyOfJobPost;
            _walletRepository.Update(wallet);
            await _walletRepository.SaveChangesAsync();
            var transaction = new Transaction
            {
                Total = moneyForJobPost,
                TypeOfTransaction = "Create job post",
                CreateBy = jobPost.Id,
                WalletId = wallet.Id
            };
            await _transactionRepository.InsertAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            var systemWalletId = new Guid("f4e64438-0fdb-44f7-8719-c69f1ac4ab67");
            var systemWallet = await _systemWalletRepository.GetFirstOrDefaultAsync(w => w.Id == systemWalletId);
            systemWallet.TotalOfSystem += moneyForSystem;
            _systemWalletRepository.Update(systemWallet);
            await _systemWalletRepository.SaveChangesAsync();
        }
        else if (requestBody.Status == (int)JobPostEnum.JobPostStatus.Cancel)
        {
            jobPost.ApproveDate = DateTime.Today.Date;
            jobPost.Status = requestBody.Status;
            jobPost.Reason = requestBody.Reason;
            _jobPostRepository.Update(jobPost);
            await _jobPostRepository.SaveChangesAsync();
        }
        var jobPostDetail = _mapper.Map<GetJobPostDetail>(jobPost);
        // // Sent noti
        // Dictionary<string, string> data = new Dictionary<string, string>()
        // {
        //     { "type", "jobPost" },
        //     { "jobPostId", jobPost.Id.ToString() }
        // };
        // await PushNotification.SendMessage(jobPost.CompanyId.ToString(), $"Bài đăng của bạn đã bị từ chối",
        //     $"Bài đăng {jobPost.Title} không được duyệt", data);

        return jobPostDetail;
    }

    public async Task DeleteJobPostAsync(Guid id)
    {
        JobPost? jobPost = await _jobPostRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (jobPost == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        jobPost.Status = (int)JobPostEnum.JobPostStatus.Hidden;
        await _jobPostRepository.SaveChangesAsync();
    }
    public async Task<int> GetTotal()
    {
        return await _jobPostRepository.GetAll().CountAsync();
    }
    
    public async Task StartJob()
    {
        var query = _jobPostRepository.Get(j =>
            (j.Status == (int)JobPostEnum.JobPostStatus.Posting && j.StartTime!.Value.Date == DateTime.Today.Date));

        var listStart = query.ToList();
        foreach (var jobPost in listStart)
        {
            jobPost.Status = (int)JobPostEnum.JobPostStatus.Default;
            Console.WriteLine($"Start: #{jobPost.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
        }
        _jobPostRepository.Update(listStart);
        await _jobPostRepository.SaveChangesAsync();
    }
    public async Task OutOfDateJob()
    {
        var query = _jobPostRepository.Get(j =>
            (j.Status == (int?)JobPostEnum.JobPostStatus.Default && j.EndTime!.Value.Date == DateTime.Today));

        var listStart = query.ToList();
        foreach (var jobPost in listStart)
        {
            jobPost.Status = (int)JobPostEnum.JobPostStatus.Hidden;
            Console.WriteLine($"Start: #{jobPost.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
        }
        _jobPostRepository.Update(listStart);
        await _jobPostRepository.SaveChangesAsync();
    }
}