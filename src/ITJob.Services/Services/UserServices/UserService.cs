using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.CompanyRepositories;
using ITJob.Entity.Repositories.EmployeeRepositories;
using ITJob.Entity.Repositories.RoleRepositories;
using ITJob.Entity.Repositories.UserRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.ViewModels.User;
using Microsoft.AspNetCore.Http;

namespace ITJob.Services.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IApplicantRepository _applicantRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IJwtHelper _jwtHelper;

    public UserService(IUserRepository userRepository, IJwtHelper jwtHelper, IRoleRepository roleRepository,
        IApplicantRepository applicantRepository, IEmployeeRepository employeeRepository, ICompanyRepository companyRepository)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
        _roleRepository = roleRepository;
        _applicantRepository = applicantRepository;
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
    }
    // public string Login(string email)
    // {
    //     User user = _userRepository.GetFirstOrDefault(u => u.Email == email);
    //     if (user == null)
    //     {
    //         throw new CException(StatusCodes.Status400BadRequest, "User not correct information!!! ");
    //     }
    //     Role role = _roleRepository.GetFirstOrDefault(u => u.Id == user.RoleId);
    //     Applicant applicant;
    //     Company company;
    //     Guid id = Guid.Empty;
    //     if (role.Name == "APPLICANT")
    //     {
    //         applicant = _applicantRepository.GetFirstOrDefault(c => c.Email == user.Email && c.Status == 1);
    //         id = applicant.Id;
    //     }
    //     else if (role.Name == "COMPANY")
    //     {
    //         company = _companyRepository.GetFirstOrDefault(c => c.Email == user.Email);
    //         if (company.Status == (int)CompanyEnum.CompanyStatus.Active)
    //         {
    //             id = company.Id;
    //         }
    //         if (company.Status == (int)CompanyEnum.CompanyStatus.Pending)
    //         {
    //             throw new CException(StatusCodes.Status400BadRequest, "Your account is not verified by email!!! ");
    //         }
    //         if (company.Status == (int)CompanyEnum.CompanyStatus.Pending)
    //         {
    //             throw new CException(StatusCodes.Status400BadRequest, "Your account is not verified by admin!!! ");
    //         }
    //     }
    //     return _jwtHelper.generateJwtToken(user, role, id);
    // }
    public string LoginApplicant(LoginApplicantModel loginApplicantModel)
    { 
        var user = _userRepository.GetFirstOrDefault(u => u.Phone == loginApplicantModel.Phone);
        var applicant = _applicantRepository.GetFirstOrDefault(c => c.Phone == loginApplicantModel.Phone);
        var isValidPassword = BCrypt.Net.BCrypt.Verify(loginApplicantModel.Password, user.Password);
        var reason = user.Reason;
        
        if (!isValidPassword || user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Phone or password not correct!!! ");
        }
        if (user.Status == (int?)UserEnum.UserStatus.Pending)
        {
            throw new CException(StatusCodes.Status400BadRequest,
                "Your account not verify, please verify your account and login again!!!");
        }
        if (user.Status == (int?)UserEnum.UserStatus.Inactive)
        {
            throw new CException(StatusCodes.Status400BadRequest,
                "Your account was banned, because "+ reason + " .!!!");
        }
        if (isValidPassword)
        {
            var role = _roleRepository.GetFirstOrDefault(u => u.Id == user.RoleId);
            return _jwtHelper.generateJwtToken(user, role, applicant.Id);
        }
        return null;
    }
    public async Task<string> Login(LoginEmailModel loginCompanyModel)
    {
        var user = await _userRepository.GetFirstOrDefaultAsync(u => u.Email == loginCompanyModel.Email);
        if (user == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Email or password not correct!!! ");
        }
        var isValidPassword = BCrypt.Net.BCrypt.Verify(loginCompanyModel.Password, user.Password);
        if (user == null || !isValidPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Email or password not correct!!! ");
        }
        if (user.Status == (int?)UserEnum.UserStatus.Verifying)
        {
            throw new CException(StatusCodes.Status400BadRequest,
                "Your account not verify, please verify your account and login again!!!");
        }
        if (user.Status == (int?)UserEnum.UserStatus.Pending)
        {
            throw new CException(StatusCodes.Status400BadRequest,
                "you have not been approved, please wait for the admin to approve and log in again!!!");
        }
        var role = await _roleRepository.GetFirstOrDefaultAsync(u => u.Id == user.RoleId);
        if (role.Name == "EMPLOYEE")
        {
            var employee = await _employeeRepository.GetFirstOrDefaultAsync(e => e.Email == user.Email);
            return _jwtHelper.generateJwtToken(user, role, employee.Id);
        }
        if (role.Name == "COMPANY")
        {
            var company = await _companyRepository.GetFirstOrDefaultAsync(c => c.Email == user.Email);
            return _jwtHelper.generateJwtToken(user, role, company.Id);
        }
        return _jwtHelper.generateJwtToken(user, role, user.Id);
    }
}