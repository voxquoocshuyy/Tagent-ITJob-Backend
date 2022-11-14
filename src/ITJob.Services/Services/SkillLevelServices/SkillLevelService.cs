using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.SkillLevelRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.SkillLevel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.SkillLevelServices;

public class SkillLevelService : ISkillLevelService
{
    private readonly ISkillLevelRepository _skillLevelRepository;
    private readonly IMapper _mapper;

    public SkillLevelService(ISkillLevelRepository skillLevelRepository, IMapper mapper)
    {
        _skillLevelRepository = skillLevelRepository;
        _mapper = mapper;
    }
    public IList<GetSkillLevelDetail> GetSkillLevelPage(PagingParam<SkillLevelEnum.SkillLevelSort> paginationModel, SearchSkillLevelModel searchSkillLevelModel)
    {
        IQueryable<SkillLevel> querySkillLevel = _skillLevelRepository.Table;
        querySkillLevel = querySkillLevel.GetWithSearch(searchSkillLevelModel);
        // Apply sort
        querySkillLevel = querySkillLevel.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        querySkillLevel = querySkillLevel.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetSkillLevelDetail>(querySkillLevel);
        return result.ToList();
    }

    public async Task<GetSkillLevelDetail> GetSkillLevelById(Guid id)
    {
        SkillLevel skillLevel = await _skillLevelRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (skillLevel == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetSkillLevelDetail>(skillLevel);
        return result;
    }

    public async Task<GetSkillLevelDetail> CreateSkillLevelAsync(CreateSkillLevelModel requestBody)
    {
        SkillLevel skillLevel = _mapper.Map<SkillLevel>(requestBody);
        if (skillLevel == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _skillLevelRepository.InsertAsync(skillLevel);
        await _skillLevelRepository.SaveChangesAsync();
        GetSkillLevelDetail skillLevelDetail = _mapper.Map<GetSkillLevelDetail>(skillLevel);
        return skillLevelDetail;
    }

    public async Task<GetSkillLevelDetail> UpdateSkillLevelAsync(Guid id, UpdateSkillLevelModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        SkillLevel skillLevel = await _skillLevelRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (skillLevel == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        skillLevel = _mapper.Map(requestBody, skillLevel);
        _skillLevelRepository.Update(skillLevel);
        await _skillLevelRepository.SaveChangesAsync();
        GetSkillLevelDetail skillLevelDetail = _mapper.Map<GetSkillLevelDetail>(skillLevel);
        return skillLevelDetail;
    }

    public async Task DeleteSkillLevelAsync(Guid id)
    {
        SkillLevel skillLevel = await _skillLevelRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (skillLevel == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _skillLevelRepository.Delete(skillLevel);
        await _skillLevelRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _skillLevelRepository.GetAll().CountAsync();
    }
}