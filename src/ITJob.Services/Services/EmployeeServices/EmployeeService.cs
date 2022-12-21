using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.EmployeeRepositories;
using ITJob.Entity.Repositories.RoleRepositories;
using ITJob.Entity.Repositories.UserRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Services.ConfirmMailServices;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ITJob.Services.Services.EmployeeServices;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMailService _mailService;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IRoleRepository roleRepository, IUserRepository userRepository, IConfiguration config, IMailService mailService)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _mailService = mailService;
    }
    public IList<GetEmployeeDetail> GetEmployeePage(PagingParam<EmployeeEnum.EmployeeSort> paginationModel, SearchEmployeeModel searchEmployeeModel)
    {
        IQueryable<Employee> queryEmployee = _employeeRepository.Table.Include(c => c.Company);
        queryEmployee = queryEmployee.GetWithSearch(searchEmployeeModel);
        // Apply sort
        queryEmployee = queryEmployee.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryEmployee = queryEmployee.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetEmployeeDetail>(queryEmployee);
        return result.ToList();
    }

    public async Task<GetEmployeeDetail> GetEmployeeById(Guid id)
    {
        Employee employee = await _employeeRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetEmployeeDetail>(employee);
        return result;
    }

    public async Task<GetEmployeeDetail> CreateEmployeeAsync(CreateEmployeeModel requestBody)
    {
        var employee = _mapper.Map<Employee>(requestBody);
        if (employee == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var employeeInDb = await _employeeRepository.GetFirstOrDefaultAsync(e => e.Email == employee.Email);
        if (employeeInDb != null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Email already exists!!! ");
        }
        var userInDb = await _userRepository.GetFirstOrDefaultAsync(e => e.Email == employee.Email);
        if (userInDb != null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Email already exists!!! ");
        }
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var password = new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        
        employee.Status = (int)EmployeeEnum.EmployeeStatus.Active;
        employee.Password = BCrypt.Net.BCrypt.HashPassword(password);
        await _employeeRepository.InsertAsync(employee);
        await _employeeRepository.SaveChangesAsync();
        var roleId = _roleRepository.GetFirstOrDefaultAsync(r => r.Name == "EMPLOYEE").Result.Id;
        var user = new User
        {
            Email = employee.Email,
            Phone = employee.Phone,
            Password = employee.Password,
            RoleId = roleId,
            EmployeeId = employee.Id,
            Status = (int)UserEnum.UserStatus.Active,
        };
        await _userRepository.InsertAsync(user);
        await _userRepository.SaveChangesAsync();
        await _mailService.SendMailToEmployeeForCreateAccount(employee.Email, password);
        GetEmployeeDetail employeeDetail = _mapper.Map<GetEmployeeDetail>(employee);
        return employeeDetail;
    }

    public async Task<GetEmployeeDetail> UpdateEmployeeAsync(Guid id, UpdateEmployeeModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Employee employee = await _employeeRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (employee == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        employee = _mapper.Map(requestBody, employee);
        _employeeRepository.Update(employee);
        await _employeeRepository.SaveChangesAsync();
        
        var user = await _userRepository.GetFirstOrDefaultAsync(alu => alu.Email == employee.Email);
        user.Phone = employee.Phone;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        
        GetEmployeeDetail employeeDetail = _mapper.Map<GetEmployeeDetail>(employee);
        return employeeDetail;
    }
    public async Task<string> UpdatePasswordEmployeeAsync(Guid id, string currentPassword, string newPassword)
    {
        var employee = await _employeeRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (employee == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(currentPassword, employee.Password);
        if (!isValidPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Current password not correct!!! ");
        }
        if (currentPassword == newPassword)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Current password equal new password!!! ");
        }
        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        employee.Password = newPasswordHash;
        _employeeRepository.Update(employee);
        await _employeeRepository.SaveChangesAsync();
        
        var roleId = _roleRepository.GetFirstOrDefaultAsync(r => r.Name == "EMPLOYEE").Result.Id;
        var tempUser = await _userRepository.GetFirstOrDefaultAsync(u => u.Email == employee.Email && u.RoleId == roleId);
        tempUser.Password = newPasswordHash;
        _userRepository.Update(tempUser);
        await _userRepository.SaveChangesAsync();
        
        return "Update password success!!!";
    }
    // public async Task<string> ForgetPasswordCompanyAsync(string email, int otp, string newPassword)
    // {
    //     var employee = await _employeeRepository.GetFirstOrDefaultAsync(alu => alu.Email == email);
    //     if (employee == null)
    //     {
    //         throw new CException(StatusCodes.Status400BadRequest, "Your email does not have a valid account!!! ");
    //     }
    //     if (otp != employee.Code)
    //     {
    //         throw new CException(StatusCodes.Status400BadRequest, "Invalid code!!! ");
    //     }
    //     var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
    //     employee.Password = newPasswordHash;
    //     _employeeRepository.Update(employee);
    //     await _employeeRepository.SaveChangesAsync();
    //     
    //     var roleId = _roleRepository.GetFirstOrDefaultAsync(r => r.Name == "EMPLOYEE").Result.Id;
    //     var tempUser = await _userRepository.GetFirstOrDefaultAsync(u => u.Phone == employee.Phone && u.RoleId == roleId);
    //     tempUser.Password = newPasswordHash;
    //     _userRepository.Update(tempUser);
    //     await _userRepository.SaveChangesAsync();
    //     
    //     return "Update password success!!!";
    // }
    public async Task DeleteEmployeeAsync(Guid id)
    {
        Employee? employee = await _employeeRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (employee == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        employee.Status = (int)EmployeeEnum.EmployeeStatus.Inactive;
        _employeeRepository.Update(employee);
        await _employeeRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _employeeRepository.GetAll().CountAsync();
    }
}