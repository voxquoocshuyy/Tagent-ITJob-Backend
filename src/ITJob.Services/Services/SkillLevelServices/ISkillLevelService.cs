using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.SkillLevel;

namespace ITJob.Services.Services.SkillLevelServices;

public interface ISkillLevelService
{
    IList<GetSkillLevelDetail> GetSkillLevelPage(PagingParam<SkillLevelEnum.SkillLevelSort> paginationModel,
        SearchSkillLevelModel searchSkillLevelModel);

    public Task<GetSkillLevelDetail> GetSkillLevelById(Guid id);

    public Task<GetSkillLevelDetail> CreateSkillLevelAsync(CreateSkillLevelModel requestBody);

    public Task<GetSkillLevelDetail> UpdateSkillLevelAsync(Guid id, UpdateSkillLevelModel requestBody);

    public Task DeleteSkillLevelAsync(Guid id);

    public Task<int> GetTotal();
}