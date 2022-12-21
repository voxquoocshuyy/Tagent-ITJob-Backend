using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Company;
using Microsoft.AspNetCore.Http;

namespace ITJob.Services.Services.CompanyServices;

public interface ICompanyService 
{
    /// <summary>
    /// Get list of all Company.
    /// </summary>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <param name="searchCompanyModel">An object contains search and filter criteria</param>
    /// <returns>List of Company.</returns>
    IList<GetCompanyDetail> GetCompanyPage(PagingParam<CompanyEnum.CompanySort> paginationModel,
        SearchCompanyModel searchCompanyModel);
    
    /// <summary>
    /// Get detail information of a Company.
    /// </summary>
    /// <param name="id">Id of Company.</param>
    /// <returns>A Company Detail.</returns>>
    public Task<GetCompanyDetail> GetCompanyById(Guid id);
    public Task<GetCompanyDetail> GetCompanyByEmail(string email);

    /// <summary>
    /// Create Company.
    /// </summary>
    /// <param name="requestBody">Model create Company request of Company.</param>
    /// <returns>A Company detail.</returns>>
    public Task<GetCompanyDetail> CreateCompanyAsync(CreateCompanyModel requestBody);

    /// <summary>
    /// Update Company.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">Model Update Company request of Company.</param>
    /// <returns>A Company Detail.</returns>>
    public Task<GetCompanyDetail> UpdateCompanyAsync(Guid id, UpdateCompanyModel requestBody);
    /// <summary>
    /// Upgrade Company.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">Model Upgrade Company request of Company.</param>
    /// <returns>A Company Detail.</returns>>
    public Task<GetCompanyDetail> UpgradeCompanyAsync(Guid id, UpgradeCompanyModel requestBody);

    public Task<string> UpdatePasswordCompanyAsync(Guid id, string currentPassword, string newPassword);
    public Task<string> ForgetPasswordCompanyAsync(string email, int otp, string newPassword);

    /// <summary>
    /// Delete Company - Change Status to Inactive
    /// </summary>
    /// <param name="id">ID of Company</param>
    /// <param name="updateReason"></param>
    /// <returns></returns>
    public Task DeleteCompanyAsync(Guid id, UpdateReason updateReason);
    
    /// <summary>
    /// Get total of Company
    /// </summary>
    /// <returns>Total of Company</returns>
    public Task<int> GetTotal();

}