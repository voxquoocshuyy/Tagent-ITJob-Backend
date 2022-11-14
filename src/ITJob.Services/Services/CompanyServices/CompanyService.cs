using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.CompanyRepositories;
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
    public CompanyService(ICompanyRepository companyRepository, IMapper mapper, IUserRepository userRepository, IFileService fileService,
        IWalletRepository walletRepository,  IConfiguration config, ITransactionRepository transactionRepository,
        ISystemWalletRepository systemWalletRepository)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _fileService = fileService;
        _walletRepository = walletRepository;
        _upgrade = double.Parse(config["SystemConfiguration:Upgrade"]);
        _transactionRepository = transactionRepository;
        _systemWalletRepository = systemWalletRepository;
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

        if (requestBody.UploadFile == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Logo of company is not null ");
        }
        company.Logo = await _fileService.UploadFile(requestBody.UploadFile);
        company.Status = (int)CompanyEnum.CompanyStatus.Pending;
        company.Premium = (int?)CompanyEnum.CompanyPremium.NotPremium;
        await _companyRepository.InsertAsync(company);
        await _companyRepository.SaveChangesAsync();
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
        if (requestBody.Premium == 1)
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
    public async Task DeleteCompanyAsync(Guid id)
    {
        Company company = await _companyRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (company == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        company.Status = (int)CompanyEnum.CompanyStatus.Inactive;
        await _companyRepository.SaveChangesAsync();
        String phone = company.Phone;
        User tempUser = await _userRepository.GetFirstOrDefaultAsync(alu => alu.Phone == phone);
        tempUser.Status = (int?)UserEnum.UserStatus.Inactive;
        await _userRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _companyRepository.GetAll().CountAsync();
    }
}