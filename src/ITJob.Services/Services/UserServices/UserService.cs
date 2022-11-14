using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.CompanyRepositories;
using ITJob.Entity.Repositories.RoleRepositories;
using ITJob.Entity.Repositories.UserRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Applicant;
using ITJob.Services.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ITJob.Services.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IApplicantRepository _applicantRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;
    private readonly IJwtHelper _jwtHelper;

    public UserService(IUserRepository userRepository, IMapper mapper, IJwtHelper jwtHelper, IRoleRepository roleRepository, ICompanyRepository companyRepository, IApplicantRepository applicantRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtHelper = jwtHelper;
        _roleRepository = roleRepository;
        _companyRepository = companyRepository;
        _applicantRepository = applicantRepository;
    }

    public IList<GetUserDetail> GetUserPage(PagingParam<UserEnum.UserSort> paginationModel, SearchUserModel searchUserModel)
    {
        IQueryable<User> queryUser = _userRepository.Table.Include(c => c.Role);
        queryUser = queryUser.GetWithSearch(searchUserModel);
        // Apply sort
        queryUser = queryUser.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryUser = queryUser.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetUserDetail>(queryUser);
        return result.ToList();
    }

    public async Task<GetUserDetail> GetUserById(Guid id)
    {
        User user = await _userRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetUserDetail>(user);
        return result;
    }
    public async Task<GetUserDetail> GetUserByEmail(string email)
    {
        User user = await _userRepository.GetFirstOrDefaultAsync(e => e.Email == email);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Email not exist!!! ");
        }
        var result = _mapper.Map<GetUserDetail>(user);
        return result;
    }
    public async Task<GetUserDetail> CreateUserAsync(CreateUserModel requestBody)
    {
        User user = _mapper.Map<User>(requestBody);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user.Status = (int?)UserEnum.UserStatus.Verifying;
        await _userRepository.InsertAsync(user);
        await _userRepository.SaveChangesAsync();
        GetUserDetail userDetail = _mapper.Map<GetUserDetail>(user);
        return userDetail;
    }

    public async Task<GetUserDetail> UpdateUserAsync(Guid id, UpdateUserModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        User user = await _userRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        user = _mapper.Map(requestBody, user);
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        GetUserDetail userDetail = _mapper.Map<GetUserDetail>(user);
        return userDetail;
    }
    public async Task<string> UpdatePasswordUserAsync(Guid id, string currentPassword, string newPassword)
    {
        User user = await _userRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(currentPassword, user.Password);
        if (!isValidPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Current password not correct!!! ");
        }
        if (currentPassword == newPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Current password equal new password!!! ");
        }
        string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.Password = newPasswordHash;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        return "Update password success!!!";
    }
    public async Task<string> ForgetPasswordUserAsync(string email, int otp, string newPassword)
    {
        User user = await _userRepository.GetFirstOrDefaultAsync(alu => alu.Email == email);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Your email does not have a valid account!!! ");
        }
        if (otp != user.Code)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Invalid code!!! ");
        }
        string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.Password = newPasswordHash;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        return "Update password success!!!";
    }
    public async Task DeleteUserAsync(Guid id)
    {
        User user = await _userRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _userRepository.Delete(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _userRepository.GetAll().CountAsync();
    }
    
    public string Login(string email)
    {
        User user = _userRepository.GetFirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "User not correct information!!! ");
        }
        Role role = _roleRepository.GetFirstOrDefault(u => u.Id == user.RoleId);
        Applicant applicant;
        Company company;
        Guid id = Guid.Empty;
        if (role.Name == "APPLICANT")
        {
            applicant = _applicantRepository.GetFirstOrDefault(c => c.Email == user.Email && c.Status == 1);
            id = applicant.Id;
        }
        else if (role.Name == "COMPANY")
        {
            company = _companyRepository.GetFirstOrDefault(c => c.Email == user.Email);
            if (company.Status == (int)CompanyEnum.CompanyStatus.Active)
            {
                id = company.Id;
            }
            if (company.Status == (int)CompanyEnum.CompanyStatus.Pending)
            {
                throw new CException(StatusCodes.Status400BadRequest, "Your account is not verified by email!!! ");
            }
            if (company.Status == (int)CompanyEnum.CompanyStatus.Pending)
            {
                throw new CException(StatusCodes.Status400BadRequest, "Your account is not verified by admin!!! ");
            }
        }
        return _jwtHelper.generateJwtToken(user, role, id);
    }
    
    
    public string LoginApplicant(LoginApplicantModel loginApplicantModel)
    {
        Applicant applicant;
        User user = _userRepository.GetFirstOrDefault(u => u.Phone == loginApplicantModel.Phone);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Phone not correct!!! ");
        }

        applicant = _applicantRepository.GetFirstOrDefault(c => c.Phone == loginApplicantModel.Phone && c.Status == 1);
        if (applicant.Status == 2)
        {
            throw new CException(StatusCodes.Status400BadRequest,
                "Your account not verify, please verify your account and login again");
        }
        bool isHavePassword = user.Password != null;
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginApplicantModel.Password, user.Password);
        if (isValidPassword && isHavePassword)
        {
            Role role = _roleRepository.GetFirstOrDefault(u => u.Id == user.RoleId);
            return _jwtHelper.generateJwtToken(user, role, applicant.Id);
        }
        if (!isValidPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Password not correct!!! ");
        }
        return null;
    }
    
    public string LoginCompany(LoginCompanyModel loginCompanyModel)
    {
        User user = _userRepository.GetFirstOrDefault(u => u.Email == loginCompanyModel.Email);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Email not correct!!! ");
        }
        bool isHavePassword = user.Password != null;
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginCompanyModel.Password, user.Password);
        if (!isValidPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Password not correct!!! ");
        }
        if (isValidPassword && isHavePassword)
        {
            if (user.Status == (int?)UserEnum.UserStatus.Verifying)
            {
                throw new CException(StatusCodes.Status400BadRequest,
                    "Your account not verify, please verify your account and login again!!!");
            }
            if (user.Status == (int?)UserEnum.UserStatus.Verified)
            {
                throw new CException(StatusCodes.Status400BadRequest,
                    "Your account not have any company, please create your company!!!");
            }
            if (user.Status == (int?)UserEnum.UserStatus.Pending)
            {
                throw new CException(StatusCodes.Status400BadRequest,
                    "Your account not approved yet, please wait for admin approved");
            }
            // Company company = _companyRepository.GetFirstOrDefault(c => c.Email == loginCompanyModel.Email);
            // if (company.Status == 2)
            // {
            //     throw new CException(StatusCodes.Status400BadRequest,
            //         "Your account not approved yet, please wait for admin approved");
            // }
            Role role = _roleRepository.GetFirstOrDefault(u => u.Id == user.RoleId);
            return _jwtHelper.generateJwtToken(user, role, user.Id);
        }
        return null;
    }
    
    public string LoginAdmin(LoginApplicantModel loginApplicantModel)
    {
        User user = _userRepository.GetFirstOrDefault(u => u.Phone == loginApplicantModel.Phone);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Phone not correct!!! ");
        }

        bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginApplicantModel.Password, user.Password);
        if (isValidPassword)
        {
            Role role = _roleRepository.GetFirstOrDefault(u => u.Id == user.RoleId);
            return _jwtHelper.generateJwtToken(user, role, user.Id);
        }   
        if (!isValidPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Password not correct!!! ");
        }
        return null;
    }
}