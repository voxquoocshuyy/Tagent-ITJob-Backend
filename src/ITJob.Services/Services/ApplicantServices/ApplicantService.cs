using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.ProfileApplicantRepositories;
using ITJob.Entity.Repositories.RoleRepositories;
using ITJob.Entity.Repositories.UserRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Services.FileServices;
using ITJob.Services.Services.Notification;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Applicant;
using ITJob.Services.ViewModels.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.ApplicantServices;

public class ApplicantService : IApplicantService
{
    private readonly IApplicantRepository _applicantRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly IWalletRepository _walletRepository;
    private readonly IProfileApplicantRepository _profileApplicantRepository;
    private readonly IRoleRepository _roleRepository;

    public ApplicantService(IApplicantRepository applicantRepository, IMapper mapper, IUserRepository userRepository, 
        IFileService fileService,  IWalletRepository walletRepository, IProfileApplicantRepository profileApplicantRepository, IRoleRepository roleRepository)
    {
        _applicantRepository = applicantRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _fileService = fileService;
        _walletRepository = walletRepository;
        _profileApplicantRepository = profileApplicantRepository;
        _roleRepository = roleRepository;
    }

    public IList<GetApplicantDetail> GetApplicantPage(PagingParam<ApplicantEnum.ApplicantSort> paginationModel, SearchApplicantModel searchApplicantModel)
    {
        IQueryable<Applicant> queryApplicant =
            _applicantRepository.Table.Include(c => c.ProfileApplicants);
        queryApplicant = queryApplicant.GetWithSearch(searchApplicantModel);
        // Apply sort
        queryApplicant = queryApplicant.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryApplicant = queryApplicant.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetApplicantDetail>(queryApplicant);
        return result.ToList();
    }

    public async Task<GetApplicantDetail> GetApplicantById(Guid id)
    {
        Applicant applicant = await _applicantRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (applicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetApplicantDetail>(applicant);
        return result;
    }
    public async Task<GetApplicantDetail> GetApplicantByPhone(string phone)
    {
        Applicant applicant = await _applicantRepository.GetFirstOrDefaultAsync(e => e.Phone == phone);
        if (applicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Phone not exist!!! ");
        }
        var result = _mapper.Map<GetApplicantDetail>(applicant);
        return result;
    }
    public async Task<GetApplicantDetail> CreateApplicantAsync(CreateApplicantModel requestBody)
    {
        Applicant applicant = _mapper.Map<Applicant>(requestBody);
        if (applicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Applicant checkApplicant = await _applicantRepository.GetFirstOrDefaultAsync(alu => alu.Phone == requestBody.Phone);
        if (checkApplicant != null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Phone has exist!!! ");
        }

        if (requestBody.UploadFile == null)
        {
            applicant.Avatar = "";
        }
        if (requestBody.UploadFile != null) applicant.Avatar = await _fileService.UploadFile(requestBody.UploadFile);
        applicant.Status = (int)ApplicantEnum.ApplicantStatus.Verifying;
        applicant.EarnMoney = (int?)ApplicantEnum.ApplicantEarnMoney.NotEarn;
        applicant.Password = BCrypt.Net.BCrypt.HashPassword(applicant.Password);
        await _applicantRepository.InsertAsync(applicant);
        await _applicantRepository.SaveChangesAsync();
        var roleId = _roleRepository.GetFirstOrDefaultAsync(r => r.Name == "APPLICANT").Result.Id;
        User tempUser = new User
        {
            Phone = applicant.Phone,
            Email = applicant.Email,
            Password = applicant.Password,
            ApplicantId = applicant.Id,
            Status = (int?)UserEnum.UserStatus.Pending,
            RoleId = roleId
        };
        await _userRepository.InsertAsync(tempUser);
        await _userRepository.SaveChangesAsync();
        GetApplicantDetail applicantDetail = _mapper.Map<GetApplicantDetail>(applicant);
        return applicantDetail;
    }

    public async Task<GetApplicantDetail> UpdateApplicantAsync(Guid id, UpdateApplicantModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Applicant applicant = await _applicantRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (applicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        applicant = _mapper.Map(requestBody, applicant);
        if (requestBody.UploadFile == null)
        {
            applicant.Avatar = applicant.Avatar;
        }
        else
        {
            applicant.Avatar = await _fileService.UploadFile(requestBody.UploadFile);
        }
        _applicantRepository.Update(applicant);
        await _applicantRepository.SaveChangesAsync();
        
        var phone = applicant.Phone;
        var user = await _userRepository.GetFirstOrDefaultAsync(alu => alu.Phone == phone);
        user.Email = applicant.Email;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        GetApplicantDetail applicantDetail = _mapper.Map<GetApplicantDetail>(applicant);
        return applicantDetail;
    }

    public async Task<string> UpdateApplicantToEarn(Guid id, UpdateEarnMoneyApplicantModel requestBody)
    {
        var applicant = await _applicantRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (applicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        applicant.EarnMoney = requestBody.EarnMoney;
        _applicantRepository.Update(applicant);
        await _applicantRepository.SaveChangesAsync();
        if (requestBody.EarnMoney == 1)
        {
            Wallet wallet = new Wallet
            {
                Balance = (double)0,
                Status = (int?)WalletEnum.WalletStatus.Active,
                ApplicantId = applicant.Id,
            };
            await _walletRepository.InsertAsync(wallet);
            await _walletRepository.SaveChangesAsync();
            // Sent noti
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "applicant" },
                { "applicantId", applicant.Id.ToString() }
            };
            await PushNotification.SendMessage(applicant.Id.ToString(), $"Tài khoản của bạn đã được cấp quyền cho việc tích luỹ đồng Tagent.",
                $"Tài khoản {applicant.Name} của bạn đã được cấp quyền cho việc tích luỹ đồng Tagent.", data);
        }
        else if (requestBody.EarnMoney == 2)
        {
            // Sent noti
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "admin" },
                { "ApplicantId", applicant.Id.ToString() }
            };
            await PushNotification.SendMessage("eba11598-4e4e-485f-8e94-eeb6a81b8e1f", $"Tài khoản của ứng viên đang yêu cầu cấp quyền cho việc tích luỹ đồng Tagent.",
                $"Tài khoản {applicant.Name} của ứng viên đang yêu cầu cấp quyền cho việc tích luỹ đồng Tagent.", data);
        }
        else if (requestBody.EarnMoney == 0)
        {
            // Sent noti
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "type", "applicant" },
                { "applicantId", applicant.Id.ToString() }
            };
            await PushNotification.SendMessage(applicant.Id.ToString(), $"Tài khoản của bạn đã không được cấp quyền cho việc tích luỹ đồng Tagent.",
                $"Tài khoản {applicant.Name} của bạn đã bị hủy quyền cho việc tích luỹ đồng Tagent.", data);
        }
        return "Update success!!!";
    }
    public async Task<string> UpdatePasswordApplicantAsync(Guid id, string currentPassword, string newPassword)
    {
        Applicant applicant = await _applicantRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (applicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(currentPassword, applicant.Password);
        if (!isValidPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Current password not correct!!! ");
        }
        if (currentPassword == newPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Current password equal new password!!! ");
        }
        string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        applicant.Password = newPasswordHash;
        _applicantRepository.Update(applicant);
        await _applicantRepository.SaveChangesAsync();
        var phone = applicant.Phone;
        var roleId = _roleRepository.GetFirstOrDefaultAsync(r => r.Name == "APPLICANT").Result.Id;
        var tempUser = await _userRepository.GetFirstOrDefaultAsync(u => u.Phone == phone && u.RoleId == roleId);
        tempUser.Password = newPasswordHash;
        _userRepository.Update(tempUser);
        await _userRepository.SaveChangesAsync();
        return "Update password success!!!";
    }
    
    public async Task<string> ForgetPasswordApplicantAsync(string phone, int otp, string newPassword)
    {
        Applicant applicant = await _applicantRepository.GetFirstOrDefaultAsync(alu => alu.Phone == phone);
        if (applicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Your phone number does not have a valid account!!! ");
        }
        if (otp != applicant.Otp)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Invalid code!!! ");
        }
        string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        applicant.Password = newPasswordHash;
        _applicantRepository.Update(applicant);
        await _applicantRepository.SaveChangesAsync();
        
        var roleId = _roleRepository.GetFirstOrDefaultAsync(r => r.Name == "APPLICANT").Result.Id;
        var tempUser = await _userRepository.GetFirstOrDefaultAsync(u => u.Phone == applicant.Phone && u.RoleId == roleId);
        tempUser.Password = newPasswordHash;
        _userRepository.Update(tempUser);
        await _userRepository.SaveChangesAsync();
        
        return "Update password success!!!";
    }
    public async Task DeleteApplicantAsync(Guid id, UpdateReason updateReason)
    {
        Applicant applicant = await _applicantRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (applicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        applicant.Status = (int)ApplicantEnum.ApplicantStatus.Inactive;
        applicant.Reason = updateReason.Reason;
        _applicantRepository.Update(applicant);
        await _applicantRepository.SaveChangesAsync();
        var applicantId = applicant.Id;
        var listProfileApplicant = _profileApplicantRepository.Get(pa => pa.ApplicantId == applicantId);
        foreach (var profileApplicant in listProfileApplicant)
        {
            profileApplicant.Status = (int)ApplicantEnum.ApplicantStatus.Inactive;
            _profileApplicantRepository.Update(profileApplicant);
        }
        await _profileApplicantRepository.SaveChangesAsync();
        String email = applicant.Email;
        User tempUser = await _userRepository.GetFirstOrDefaultAsync(alu => alu.Email == email);
        tempUser.Status = (int?)UserEnum.UserStatus.Inactive;
        tempUser.Reason = updateReason.Reason;
        _userRepository.Update(tempUser);
        await _userRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _applicantRepository.GetAll().CountAsync();
    }
}