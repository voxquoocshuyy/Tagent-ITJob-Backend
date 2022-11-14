using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.SkillGroupRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.SkillGroup;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.SkillGroupServices;

public class SkillGroupService : ISkillGroupService
{
    private readonly ISkillGroupRepository _skillGroupRepository;
    private readonly IMapper _mapper;

    public SkillGroupService(ISkillGroupRepository skillGroupRepository, IMapper mapper)
    {
        _skillGroupRepository = skillGroupRepository;
        _mapper = mapper;
    }

    public IList<GetSkillGroupDetail> GetSkillGroupPage(PagingParam<SkillGroupEnum.SkillGroupSort> paginationModel, SearchSkillGroupModel searchSkillGroupModel)
    {
        IQueryable<SkillGroup> querySkillGroup = _skillGroupRepository.Table.Include(c => c.Skills);
        querySkillGroup = querySkillGroup.GetWithSearch(searchSkillGroupModel);
        // Apply sort
        querySkillGroup = querySkillGroup.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        querySkillGroup = querySkillGroup.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetSkillGroupDetail>(querySkillGroup);
        return result.ToList();
    }

    public async Task<GetSkillGroupDetail> GetSkillGroupById(Guid id)
    {
        SkillGroup skillGroup = await _skillGroupRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (skillGroup == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetSkillGroupDetail>(skillGroup);
        return result;
    }

    public async Task<GetSkillGroupDetail> CreateSkillGroupAsync(CreateSkillGroupModel requestBody)
    {
        SkillGroup skillGroup = _mapper.Map<SkillGroup>(requestBody);
        if (skillGroup == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _skillGroupRepository.InsertAsync(skillGroup);
        await _skillGroupRepository.SaveChangesAsync();
        GetSkillGroupDetail skillGroupDetail = _mapper.Map<GetSkillGroupDetail>(skillGroup);
        return skillGroupDetail;
    }

    public async Task<GetSkillGroupDetail> UpdateSkillGroupAsync(Guid id, UpdateSkillGroupModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        SkillGroup skillGroup = await _skillGroupRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (skillGroup == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        skillGroup = _mapper.Map(requestBody, skillGroup);
        _skillGroupRepository.Update(skillGroup);
        await _skillGroupRepository.SaveChangesAsync();
        GetSkillGroupDetail skillGroupDetail = _mapper.Map<GetSkillGroupDetail>(skillGroup);
        return skillGroupDetail;
    }

    public async Task DeleteSkillGroupAsync(Guid id)
    {
        SkillGroup skillGroup = await _skillGroupRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (skillGroup == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _skillGroupRepository.Delete(skillGroup);
        await _skillGroupRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _skillGroupRepository.GetAll().CountAsync();
    }
}