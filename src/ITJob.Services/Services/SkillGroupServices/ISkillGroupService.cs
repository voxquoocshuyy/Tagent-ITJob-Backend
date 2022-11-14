using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.SkillGroup;

namespace ITJob.Services.Services.SkillGroupServices;

public interface ISkillGroupService
{
    /// <summary>
    /// Get list of all SkillGroup.
    /// </summary>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <param name="searchSkillGroupModel">An object contains search and filter criteria</param>
    /// <returns>List of SkillGroup.</returns>
    IList<GetSkillGroupDetail> GetSkillGroupPage(PagingParam<SkillGroupEnum.SkillGroupSort> paginationModel,
        SearchSkillGroupModel searchSkillGroupModel);
    
    /// <summary>
    /// Get detail information of a SkillGroup.
    /// </summary>
    /// <param name="id">Id of SkillGroup.</param>
    /// <returns>A SkillGroup Detail.</returns>>
    public Task<GetSkillGroupDetail> GetSkillGroupById(Guid id);
    
    /// <summary>
    /// Create SkillGroup.
    /// </summary>
    /// <param name="requestBody">Model create SkillGroup request of SkillGroup.</param>
    /// <returns>A SkillGroup detail.</returns>>
    public Task<GetSkillGroupDetail> CreateSkillGroupAsync(CreateSkillGroupModel requestBody);

    /// <summary>
    /// Update SkillGroup.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">Model Update SkillGroup request of SkillGroup.</param>
    /// <returns>A SkillGroup Detail.</returns>>
    public Task<GetSkillGroupDetail> UpdateSkillGroupAsync(Guid id, UpdateSkillGroupModel requestBody);
        
    /// <summary>
    /// Delete SkillGroup - Change Status to Inactive
    /// </summary>
    /// <param name="id">ID of SkillGroup</param>
    /// <returns></returns>
    public Task DeleteSkillGroupAsync(Guid id);
    
    /// <summary>
    /// Get total of SkillGroup
    /// </summary>
    /// <returns>Total of SkillGroup</returns>
    public Task<int> GetTotal();
}