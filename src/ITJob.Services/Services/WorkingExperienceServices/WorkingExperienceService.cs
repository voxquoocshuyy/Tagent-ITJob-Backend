using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.WorkingExperienceRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.WorkingExperience;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.WorkingExperienceServices;

public class WorkingExperienceService : IWorkingExperienceService
{
    private readonly IWorkingExperienceRepository _workingExperienceRepository;
    private readonly IMapper _mapper;

    public WorkingExperienceService(IWorkingExperienceRepository workingExperienceRepository, IMapper mapper)
    {
        _workingExperienceRepository = workingExperienceRepository;
        _mapper = mapper;
    }
    
    public IList<GetWorkingExperienceDetail> GetWorkingExperiencePage(PagingParam<WorkingExperienceEnum.WorkingExperienceSort> paginationModel, SearchWorkingExperienceModel searchWorkingExperienceModel)
    {
        IQueryable<WorkingExperience> queryWorkingExperience = _workingExperienceRepository.Table.Include(c => c.ProfileApplicant);
        queryWorkingExperience = queryWorkingExperience.GetWithSearch(searchWorkingExperienceModel);
        // Apply sort
        queryWorkingExperience = queryWorkingExperience.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryWorkingExperience = queryWorkingExperience.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetWorkingExperienceDetail>(queryWorkingExperience);
        return result.ToList();
    }

    public async Task<GetWorkingExperienceDetail> GetWorkingExperienceById(Guid id)
    {
        WorkingExperience workingExperience = await _workingExperienceRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (workingExperience == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetWorkingExperienceDetail>(workingExperience);
        return result;
    }

    public async Task<GetWorkingExperienceDetail> CreateWorkingExperienceAsync(CreateWorkingExperienceModel requestBody)
    {
        WorkingExperience workingExperience = _mapper.Map<WorkingExperience>(requestBody);
        if (workingExperience == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _workingExperienceRepository.InsertAsync(workingExperience);
        await _workingExperienceRepository.SaveChangesAsync();
        GetWorkingExperienceDetail workingExperienceDetail = _mapper.Map<GetWorkingExperienceDetail>(workingExperience);
        return workingExperienceDetail;
    }

    public async Task<GetWorkingExperienceDetail> UpdateWorkingExperienceAsync(Guid id, UpdateWorkingExperienceModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        WorkingExperience workingExperience = await _workingExperienceRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (workingExperience == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        workingExperience = _mapper.Map(requestBody, workingExperience);
        _workingExperienceRepository.Update(workingExperience);
        await _workingExperienceRepository.SaveChangesAsync();
        GetWorkingExperienceDetail workingExperienceDetail = _mapper.Map<GetWorkingExperienceDetail>(workingExperience);
        return workingExperienceDetail;
    }

    public async Task DeleteWorkingExperienceAsync(Guid id)
    {
        WorkingExperience workingExperience = await _workingExperienceRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (workingExperience == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _workingExperienceRepository.Delete(workingExperience);
        await _workingExperienceRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _workingExperienceRepository.GetAll().CountAsync();
    }
}