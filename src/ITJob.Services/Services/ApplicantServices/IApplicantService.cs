using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Applicant;
using ITJob.Services.ViewModels.Company;

namespace ITJob.Services.Services.ApplicantServices;

public interface IApplicantService
{
    /// <summary>
    /// Get list of all Applicant.
    /// </summary>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <param name="searchApplicantModel">An object contains search and filter criteria</param>
    /// <returns>List of Applicant.</returns>
    IList<GetApplicantDetail> GetApplicantPage(PagingParam<ApplicantEnum.ApplicantSort> paginationModel,
        SearchApplicantModel searchApplicantModel);
    
    /// <summary>
    /// Get detail information of a Applicant.
    /// </summary>
    /// <param name="id">Id of Applicant.</param>
    /// <returns>A Applicant Detail.</returns>>
    public Task<GetApplicantDetail> GetApplicantById(Guid id);
    public Task<GetApplicantDetail> GetApplicantByPhone(string phone);
    
    /// <summary>
    /// Create Applicant.
    /// </summary>
    /// <param name="requestBody">Model create Applicant request of Applicant.</param>
    /// <returns>A Applicant detail.</returns>>
    public Task<GetApplicantDetail> CreateApplicantAsync(CreateApplicantModel requestBody);

    /// <summary>
    /// Update Applicant.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">Model Update Applicant request of Applicant.</param>
    /// <returns>A Applicant Detail.</returns>>
    public Task<GetApplicantDetail> UpdateApplicantAsync(Guid id, UpdateApplicantModel requestBody);

    /// <summary>
    /// Update password.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="currentPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns>Msg</returns>>
    public Task<string> UpdatePasswordApplicantAsync(Guid id, string currentPassword, string newPassword);

    public Task<string> UpdateApplicantToEarn(Guid id, UpdateEarnMoneyApplicantModel requestBody);

    /// <summary>
    /// Update password.
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="otp"></param>
    /// <param name="newPassword"></param>
    /// <returns>Msg</returns>>
    public Task<string> ForgetPasswordApplicantAsync(string phone, int otp, string newPassword);

    /// <summary>
    /// Delete Applicant - Change Status to Inactive
    /// </summary>
    /// <param name="id">ID of Applicant</param>
    /// <param name="updateReason"></param>
    /// <returns></returns>
    public Task DeleteApplicantAsync(Guid id, UpdateReason updateReason);
    
    /// <summary>
    /// Get total of Applicant
    /// </summary>
    /// <returns>Total of Applicant</returns>
    public Task<int> GetTotal();
}