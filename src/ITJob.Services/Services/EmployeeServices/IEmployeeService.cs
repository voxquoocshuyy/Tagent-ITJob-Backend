using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Employee;

namespace ITJob.Services.Services.EmployeeServices;

public interface IEmployeeService
{
    IList<GetEmployeeDetail> GetEmployeePage(PagingParam<EmployeeEnum.EmployeeSort> paginationModel, SearchEmployeeModel searchEmployeeModel);

    public Task<GetEmployeeDetail> GetEmployeeById(Guid id);

    public Task<GetEmployeeDetail> CreateEmployeeAsync(CreateEmployeeModel requestBody);

    public Task<GetEmployeeDetail> UpdateEmployeeAsync(Guid id, UpdateEmployeeModel requestBody);
    public Task<string> UpdatePasswordEmployeeAsync(Guid id, string currentPassword, string newPassword);

    public Task DeleteEmployeeAsync(Guid id);

    public Task<int> GetTotal();   
}