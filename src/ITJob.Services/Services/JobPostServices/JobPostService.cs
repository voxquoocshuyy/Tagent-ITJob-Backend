using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.JobPostRepositories;
using ITJob.Entity.Repositories.LikeRepositories;
using ITJob.Entity.Repositories.SystemWalletRepositories;
using ITJob.Entity.Repositories.TransactionRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Services.Notification;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.JobPost;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.JobPostServices;

public class JobPostService : IJobPostService
{
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IMapper _mapper;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ILikeRepository _likeRepository;

    public JobPostService(IJobPostRepository jobPostRepository, IMapper mapper, ITransactionRepository
        transactionRepository, IWalletRepository walletRepository, ILikeRepository likeRepository)
    {
        _jobPostRepository = jobPostRepository;
        _mapper = mapper;
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _likeRepository = likeRepository;
    }
    public IList<GetJobPostDetail> GetJobPostPage(PagingParam<JobPostEnum.JobPostSort> paginationModel, SearchJobPostModel searchJobPostModel)
    {
        IQueryable<JobPost> queryJobPost = _jobPostRepository.Table
            // .Include(c => c.Company)
            .Include(c => c.Likes)
            .Include(c => c.AlbumImages)
            .Include(c => c.JobPostSkills)
            .Include(c => c.JobPosition)
            .Include(c => c.WorkingStyle);
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
                .Where(l => l.IsJobPostLike == true && l.Match == null)
                .Include(p => p.JobPost).Select(p => p.JobPost);
        queryJobPost = queryJobPost.GetWithSearch(searchProfileApplicantModel);
        // Apply sort
        queryJobPost = queryJobPost.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryJobPost = queryJobPost.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetJobPostDetail>(queryJobPost);
        return result.ToList();
    }
    public IList<GetJobPostDetail> GetJobPostProfileApplicantLikePage(
        PagingParam<JobPostEnum.JobPostSort> paginationModel, 
        SearchJobPostModel searchProfileApplicantModel, Guid profileApplicantId)
    {
        IQueryable<JobPost?> queryJobPost =
            _likeRepository.Get(l => l.ProfileApplicantId == profileApplicantId)
                .Where(l => l.IsProfileApplicantLike == true && l.Match == null)
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
        //update company
        jobPost.Status = (int)JobPostEnum.JobPostStatus.Pending;
        jobPost.Money = requestBody.MoneyForJobPost;
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
    public async Task<GetJobPostDetail> UpdateJobPostExpiredAsync(Guid id, UpdateJobPostExpriredModel requestBody)
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
        jobPost.Status = (int)JobPostEnum.JobPostStatus.Hidden;
        _jobPostRepository.Update(jobPost);
        await _jobPostRepository.SaveChangesAsync();
        //update wallet
        var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.CompanyId == jobPost.CompanyId);
        var transaction = new Transaction
        {
            Total = jobPost.Money,
            TypeOfTransaction = "Return money",
            CreateBy = jobPost.Id,
            WalletId = wallet.Id
        };
        await _transactionRepository.InsertAsync(transaction);
        await _transactionRepository.SaveChangesAsync();
        wallet.Balance += jobPost.Money;
        jobPost.Money = 0;
        _walletRepository.Update(wallet);
        await _walletRepository.SaveChangesAsync();
        // Sent notification
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "type", "jobPost" },
            { "jobPostId", jobPost.Id.ToString() }
        };
        await PushNotification.SendMessage(jobPost.EmployeeId.ToString(), $"Bài đăng của bạn đã hết hạn.",
            $"Bài đăng {jobPost.Title} đã hết hạn. Chúng tôi sẽ đã hoàn {jobPost.Money} vào ví của bạn.", data);
        GetJobPostDetail jobPostDetail = _mapper.Map<GetJobPostDetail>(jobPost);
        return jobPostDetail;
    }
    public async Task<GetJobPostDetail> UpdateJobPostMoneyAsync(Guid id, UpdateJobPostMoneyModel requestBody)
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
        //update money of job post
        var moneyOfJobPost = requestBody.MoneyForJobPost;
        // var moneyForJobPost = (moneyOfJobPost * 60) / 100;
        // var moneyForSystem = moneyOfJobPost - moneyForJobPost;
        jobPost.Money += moneyOfJobPost;
        _jobPostRepository.Update(jobPost);
        await _jobPostRepository.SaveChangesAsync();
        //update system wallet
        // var systemWalletId = new Guid("f4e64438-0fdb-44f7-8719-c69f1ac4ab67");
        // var systemWallet = await _systemWalletRepository.GetFirstOrDefaultAsync(w => w.Id == systemWalletId);
        // systemWallet.TotalOfSystem += (double)moneyForSystem;
        // _systemWalletRepository.Update(systemWallet);
        // await _systemWalletRepository.SaveChangesAsync();
        //update wallet
        var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.CompanyId == jobPost.CompanyId);
        var transaction = new Transaction
        {
            Total = requestBody.MoneyForJobPost,
            TypeOfTransaction = "Top up for job post",
            CreateBy = jobPost.Id,
            WalletId = wallet.Id
        };
        await _transactionRepository.InsertAsync(transaction);
        await _transactionRepository.SaveChangesAsync();
        //minus money
        wallet.Balance -= requestBody.MoneyForJobPost;
        _walletRepository.Update(wallet);
        await _walletRepository.SaveChangesAsync();
        
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
        
        var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.CompanyId == jobPost.CompanyId);
        var balance = wallet.Balance;
        var moneyOfJobPost = jobPost.Money;
        // var moneyForJobPost = (moneyOfJobPost * 60) / 100;
        // var moneyForSystem = moneyOfJobPost - moneyForJobPost;
        if (requestBody.Status == (int)JobPostEnum.JobPostStatus.Posting || requestBody.Status == (int)JobPostEnum.JobPostStatus.Default)
        {
            jobPost.ApproveDate = DateTime.Now;
            jobPost.Status = requestBody.Status;
            jobPost.Money = moneyOfJobPost;
            _jobPostRepository.Update(jobPost);
            await _jobPostRepository.SaveChangesAsync();
            
            //minus money in wallet of company
            if (balance < jobPost.Money)
            {
                throw new CException(StatusCodes.Status400BadRequest, "The company's wallet does not " +
                                                                      "have enough funds to browse this post!!! ");
            }
        
            wallet.Balance = balance - moneyOfJobPost;
            _walletRepository.Update(wallet);   
            await _walletRepository.SaveChangesAsync();
        
            //create transaction
            var transaction = new Transaction
            {
                Total = moneyOfJobPost,
                TypeOfTransaction = "Create job post",
                CreateBy = jobPost.Id,
                WalletId = wallet.Id
            };
            await _transactionRepository.InsertAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            
            // var systemWalletId = new Guid("f4e64438-0fdb-44f7-8719-c69f1ac4ab67");
            // var systemWallet = await _systemWalletRepository.GetFirstOrDefaultAsync(w => w.Id == systemWalletId);
            // systemWallet.TotalOfSystem += (double)moneyForSystem;
            // _systemWalletRepository.Update(systemWallet);
            // await _systemWalletRepository.SaveChangesAsync();
            // Sent notification
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "jobPost" },
                { "jobPostId", jobPost.Id.ToString() }
            };
            await PushNotification.SendMessage(jobPost.EmployeeId.ToString(), $"Bài đăng của bạn đã được duyệt.",
                $"Bài đăng {jobPost.Title} đã được duyệt.", data);
        }
        else if (requestBody.Status == (int)JobPostEnum.JobPostStatus.Cancel)
        {
            jobPost.Status = requestBody.Status;
            jobPost.Reason = requestBody.Reason;
            // //update wallet
            // wallet.Balance = balance + moneyOfJobPost;
            // _walletRepository.Update(wallet);   
            // await _walletRepository.SaveChangesAsync();
            // //create transaction
            // var transaction = new Transaction
            // {
            //     Total = moneyOfJobPost,
            //     TypeOfTransaction = "Return money",
            //     CreateBy = jobPost.Id,
            //     WalletId = wallet.Id
            // };
            // await _transactionRepository.InsertAsync(transaction);
            // await _transactionRepository.SaveChangesAsync();
            
            _jobPostRepository.Update(jobPost);
            await _jobPostRepository.SaveChangesAsync();
            // Sent notification
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "jobPost" },
                { "jobPostId", jobPost.Id.ToString() }
            };
            await PushNotification.SendMessage(jobPost.EmployeeId.ToString(), $"Bài đăng của bạn đã bị từ chối.",
                $"Bài đăng {jobPost.Title} không được duyệt.", data);
        }
        var jobPostDetail = _mapper.Map<GetJobPostDetail>(jobPost);
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
            (j.Status == (int)JobPostEnum.JobPostStatus.Posting && j.StartTime!.Value.Date == DateTime.Today.Date)).ToList();
        foreach (var jobPost in query)
        {
            //update status job post
            jobPost.Status = (int)JobPostEnum.JobPostStatus.Default;
            _jobPostRepository.Update(jobPost);
            Console.WriteLine($"Start: #{jobPost.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
            // Sent notification
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "jobPost" },
                { "jobPostId", jobPost.Id.ToString() }
            };
            await PushNotification.SendMessage(jobPost.EmployeeId.ToString(), $"Bài đăng của bạn đã được đăng.",
                $"Bài đăng {jobPost.Title} đã được đăng.", data);
        }
        await _jobPostRepository.SaveChangesAsync();
    }
    public async Task OutOfDateJob()
    {
        var query = _jobPostRepository.Get(j =>
            (j.Status == (int?)JobPostEnum.JobPostStatus.Default && j.EndTime!.Value.Date == DateTime.Today.Date)).ToList();

        foreach (var jobPost in query)
        {
            //update status job post
            jobPost.Status = (int)JobPostEnum.JobPostStatus.Hidden;
            Console.WriteLine($"End: #{jobPost.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
            //update wallet
            var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.CompanyId == jobPost.CompanyId);
            wallet.Balance += jobPost.Money;
            _walletRepository.Update(wallet);
            await _walletRepository.SaveChangesAsync();
            //create transaction
            var transaction = new Transaction
            {
                Total = jobPost.Money,
                TypeOfTransaction = "Return money",
                CreateBy = jobPost.Id,
                WalletId = wallet.Id
            };
            await _transactionRepository.InsertAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            //update money job post
            jobPost.Money = 0;
            _jobPostRepository.Update(jobPost);
            //Sent notification
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "jobPost" },
                { "jobPostId", jobPost.Id.ToString() }
            };
            await PushNotification.SendMessage(jobPost.EmployeeId.ToString(), $"Bài đăng của bạn đã hết hạn.",
                $"Bài đăng {jobPost.Title} đã hết hạn.", data);
        }
        await _jobPostRepository.SaveChangesAsync();
    }
    public async Task OutOfMoney()
    {
        var query = _jobPostRepository.Get(j =>
            (j.Status == (int?)JobPostEnum.JobPostStatus.Default && j.Money <= (double)10));
    
        var listEnd = query.ToList();
        foreach (var jobPost in listEnd)
        {
            //update status job post
            jobPost.Status = (int)JobPostEnum.JobPostStatus.Hidden;
            Console.WriteLine($"End: #{jobPost.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
            // Sent notification
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "jobPost" },
                { "jobPostId", jobPost.Id.ToString() }
            };
            await PushNotification.SendMessage(jobPost.EmployeeId.ToString(), $"Bài đăng của bạn đã hết phí duy trì.",
                $"Bài đăng {jobPost.Title} đã hết phí duy trì.", data);
        }
        _jobPostRepository.Update(listEnd);
        await _jobPostRepository.SaveChangesAsync();
    }
}