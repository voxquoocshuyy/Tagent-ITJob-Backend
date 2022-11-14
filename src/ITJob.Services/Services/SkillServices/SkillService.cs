using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.SkillRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Skill;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.SkillServices;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _skillRepository;
    private readonly IMapper _mapper;

    public SkillService(ISkillRepository skillRepository, IMapper mapper)
    {
        _skillRepository = skillRepository;
        _mapper = mapper;
    }
    public IList<GetSkillDetail> GetSkillPage(PagingParam<SkillEnum.SkillSort> paginationModel, SearchSkillModel searchSkillModel)
    {
        IQueryable<Skill> querySkill = _skillRepository.Table.Include(c => c.SkillGroup);
        querySkill = querySkill.GetWithSearch(searchSkillModel);
        // Apply sort
        querySkill = querySkill.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        querySkill = querySkill.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetSkillDetail>(querySkill);
        return result.ToList();
    }

    public async Task<GetSkillDetail> GetSkillById(Guid id)
    {
        Skill skill = await _skillRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (skill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetSkillDetail>(skill);
        return result;
    }

    public async Task<GetSkillDetail> CreateSkillAsync(CreateSkillModel requestBody)
    {
        Skill skill = _mapper.Map<Skill>(requestBody);
        if (skill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _skillRepository.InsertAsync(skill);
        await _skillRepository.SaveChangesAsync();
        GetSkillDetail skillDetail = _mapper.Map<GetSkillDetail>(skill);
        return skillDetail;
    }

    public async Task<GetSkillDetail> UpdateSkillAsync(Guid id, UpdateSkillModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Skill skill = await _skillRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (skill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        skill = _mapper.Map(requestBody, skill);
        _skillRepository.Update(skill);
        await _skillRepository.SaveChangesAsync();
        GetSkillDetail skillDetail = _mapper.Map<GetSkillDetail>(skill);
        return skillDetail;
    }

    public async Task DeleteSkillAsync(Guid id)
    {
        Skill skill = await _skillRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (skill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _skillRepository.Delete(skill);
        await _skillRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _skillRepository.GetAll().CountAsync();
    }
}