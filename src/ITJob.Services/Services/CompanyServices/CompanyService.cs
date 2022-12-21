using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.CompanyRepositories;
using ITJob.Entity.Repositories.EmployeeRepositories;
using ITJob.Entity.Repositories.JobPostRepositories;
using ITJob.Entity.Repositories.RoleRepositories;
using ITJob.Entity.Repositories.SystemWalletRepositories;
using ITJob.Entity.Repositories.TransactionRepositories;
using ITJob.Entity.Repositories.UserRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using User = ITJob.Entity.Entities.User;
using ITJob.Services.Services.FileServices;
using ITJob.Services.Services.Notification;
using Microsoft.Extensions.Configuration;

namespace ITJob.Services.Services.CompanyServices;

public class CompanyService : ICompanyService
{
    private readonly double _upgrade;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ISystemWalletRepository _systemWalletRepository;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public CompanyService(ICompanyRepository companyRepository, IMapper mapper, IUserRepository userRepository, IFileService fileService,
        IWalletRepository walletRepository,  IConfiguration config, ITransactionRepository transactionRepository,
        ISystemWalletRepository systemWalletRepository, IJobPostRepository jobPostRepository, IRoleRepository roleRepository,
        IEmployeeRepository employeeRepository)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _fileService = fileService;
        _walletRepository = walletRepository;
        _upgrade = double.Parse(config["SystemConfiguration:Upgrade"]);
        _transactionRepository = transactionRepository;
        _systemWalletRepository = systemWalletRepository;
        _jobPostRepository = jobPostRepository;
        _roleRepository = roleRepository;
        _employeeRepository = employeeRepository;
    }
    public IList<GetCompanyDetail> GetCompanyPage(PagingParam<CompanyEnum.CompanySort> paginationModel, SearchCompanyModel searchCompanyModel)
    {
        IQueryable<Company> queryCompany = _companyRepository.Table.Include(c => c.JobPosts);
        queryCompany = queryCompany.GetWithSearch(searchCompanyModel);
        // Apply sort
        queryCompany = queryCompany.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryCompany = queryCompany.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetCompanyDetail>(queryCompany);
        return result.ToList();
    }

    public async Task<GetCompanyDetail> GetCompanyById(Guid id)
    {
        Company company = await _companyRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (company == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetCompanyDetail>(company);
        return result;
    }

    public async Task<GetCompanyDetail> GetCompanyByEmail(string email)
    {
        Company company = await _companyRepository.GetFirstOrDefaultAsync(e => e.Email == email);
        if (company == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Email not exist!!! ");
        }
        var result = _mapper.Map<GetCompanyDetail>(company);
        return result;
    }
    
    public async Task<GetCompanyDetail> CreateCompanyAsync(CreateCompanyModel requestBody)
    {
        Company company = _mapper.Map<Company>(requestBody);
        if (company == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Company checkCompany = await _companyRepository.GetFirstOrDefaultAsync(alu => alu.Email == requestBody.Email);
        if (checkCompany != null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Email has exist!!! ");
        }
        var userInDb = await _userRepository.GetFirstOrDefaultAsync(e => e.Email == company.Email);
        if (userInDb != null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Email already exists!!! ");
        }

        if (requestBody.UploadFile == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Logo of company is not null ");
        }
        company.Logo = await _fileService.UploadFile(requestBody.UploadFile);
        company.Status = (int)CompanyEnum.CompanyStatus.Verifying;
        company.IsPremium = false;
        company.Password = BCrypt.Net.BCrypt.HashPassword(company.Password);
        await _companyRepository.InsertAsync(company);
        await _companyRepository.SaveChangesAsync();
        var roleId = _roleRepository.GetFirstOrDefaultAsync(r => r.Name == "COMPANY").Result.Id;
        var user = new User
        {
            Email = company.Email,
            Phone = company.Phone,
            Password = company.Password,
            RoleId = roleId,
            CompanyId = company.Id,
            Status = (int)UserEnum.UserStatus.Verifying,
        };
        await _userRepository.InsertAsync(user);
        await _userRepository.SaveChangesAsync();
        // Sent noti
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "type", "admin" },
            { "CompanyId", company.Id.ToString() }
        };
        await PushNotification.SendMessage("eba11598-4e4e-485f-8e94-eeb6a81b8e1f", $"Xét duyệt tài khoản công ty .",
            $"Công ty {company.Name}  đang yêu cầu xét duyệt.", data);
        
        var companyDetail = _mapper.Map<GetCompanyDetail>(company);
        return companyDetail;
    }

    public async Task<GetCompanyDetail> UpdateCompanyAsync(Guid id, UpdateCompanyModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var company = await _companyRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (company == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        if (requestBody.UploadFile == null)
        {
            company.Logo = company.Logo;
        }
        else
        {
            company.Logo = await _fileService.UploadFile(requestBody.UploadFile);
        }
        company = _mapper.Map(requestBody, company);
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
        var companyDetail = _mapper.Map<GetCompanyDetail>(company);
        return companyDetail;
    }
    public async Task<GetCompanyDetail> UpgradeCompanyAsync(Guid id, UpgradeCompanyModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var company = await _companyRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (company == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        company = _mapper.Map(requestBody, company);
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
        if (requestBody.IsPremium == true)
        {
            var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.CompanyId == company.Id);
            wallet.Balance -= _upgrade;
            _walletRepository.Update(wallet);
            await _walletRepository.SaveChangesAsync();
            
            Transaction transaction = new Transaction
            {
                Total = _upgrade,
                TypeOfTransaction = "Upgrade",
                CreateBy = company.Id,
                WalletId = wallet.Id
            };
            await _transactionRepository.InsertAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            var systemWalletId = new Guid("f4e64438-0fdb-44f7-8719-c69f1ac4ab67");
            var systemWallet = await _systemWalletRepository.GetFirstOrDefaultAsync(w => w.Id == systemWalletId);
            systemWallet.TotalOfSystem += _upgrade;
            _systemWalletRepository.Update(systemWallet);
            await _systemWalletRepository.SaveChangesAsync();
        }
        var companyDetail = _mapper.Map<GetCompanyDetail>(company);
        return companyDetail;
    }
    
    public async Task<string> UpdatePasswordCompanyAsync(Guid id, string currentPassword, string newPassword)
    {
        var company = await _companyRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (company == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(currentPassword, company.Password);
        if (!isValidPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Current password not correct!!! ");
        }
        if (currentPassword == newPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Current password equal new password!!! ");
        }
        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        company.Password = newPasswordHash;
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
        
        var roleId = _roleRepository.GetFirstOrDefaultAsync(r => r.Name == "COMPANY").Result.Id;
        var tempUser = await _userRepository.GetFirstOrDefaultAsync(u => u.Email == company.Email && u.RoleId == roleId);
        tempUser.Password = newPasswordHash;
        _userRepository.Update(tempUser);
        await _userRepository.SaveChangesAsync();
        
        return "Update password success!!!";
    }
    public async Task<string> ForgetPasswordCompanyAsync(string email, int otp, string newPassword)
    {
        var company = await _companyRepository.GetFirstOrDefaultAsync(alu => alu.Email == email);
        if (company == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Your email does not have a valid account!!! ");
        }
        if (otp != company.Code)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Invalid code!!! ");
        }
        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        company.Password = newPasswordHash;
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
        
        var roleId = _roleRepository.GetFirstOrDefaultAsync(r => r.Name == "COMPANY").Result.Id;
        var tempUser = await _userRepository.GetFirstOrDefaultAsync(u => u.Phone == company.Phone && u.RoleId == roleId);
        tempUser.Password = newPasswordHash;
        _userRepository.Update(tempUser);
        await _userRepository.SaveChangesAsync();
        
        return "Update password success!!!";
    }
    public async Task DeleteCompanyAsync(Guid id, UpdateReason updateReason)
    {
        Company company = await _companyRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (company == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        company.Status = (int)CompanyEnum.CompanyStatus.Inactive;
        company.Reason = updateReason.Reason;
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
        
        var companyId = company.Id;
        var user = await _userRepository.GetFirstOrDefaultAsync(alu => alu.CompanyId == companyId);
        user.Status = (int?)UserEnum.UserStatus.Inactive;
        user.Reason = updateReason.Reason;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        
        var listEmployee = _employeeRepository.Get(jp => jp.CompanyId == companyId);
        foreach (var employee in listEmployee)
        {
            employee.Status = (int)EmployeeEnum.EmployeeStatus.Inactive;
            employee.Reason = updateReason.Reason;
            _employeeRepository.Update(employee);
        }
        await _employeeRepository.SaveChangesAsync();
        
        var listJobPost = _jobPostRepository.Get(jp => jp.CompanyId == companyId);
        foreach (var jobPost in listJobPost)
        {
            jobPost.Status = (int)JobPostEnum.JobPostStatus.Hidden;
            _jobPostRepository.Update(jobPost);
        }
        await _jobPostRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _companyRepository.GetAll().CountAsync();
    }
}