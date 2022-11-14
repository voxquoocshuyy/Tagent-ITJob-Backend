using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Skill;

namespace ITJob.Services.Services.SkillServices;

public interface ISkillService
{
    IList<GetSkillDetail> GetSkillPage(PagingParam<SkillEnum.SkillSort> paginationModel,
        SearchSkillModel searchSkillModel);

    public Task<GetSkillDetail> GetSkillById(Guid id);

    public Task<GetSkillDetail> CreateSkillAsync(CreateSkillModel requestBody);

    public Task<GetSkillDetail> UpdateSkillAsync(Guid id, UpdateSkillModel requestBody);

    public Task DeleteSkillAsync(Guid id);

    public Task<int> GetTotal();
}