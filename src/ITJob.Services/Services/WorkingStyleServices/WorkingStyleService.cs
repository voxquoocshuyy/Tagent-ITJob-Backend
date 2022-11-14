using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.WorkingStyleRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.WorkingStyle;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.WorkingStyleServices;

public class WorkingStyleService : IWorkingStyleService
{
    private readonly IWorkingStyleRepository _workingStyleRepository;
    private readonly IMapper _mapper;

    public WorkingStyleService(IWorkingStyleRepository workingStyleRepository, IMapper mapper)
    {
        _workingStyleRepository = workingStyleRepository;
        _mapper = mapper;
    }
    public IList<GetWorkingStyleDetail> GetWorkingStylePage(PagingParam<WorkingStyleEnum.WorkingStyleSort> paginationModel, SearchWorkingStyleModel searchWorkingStyleModel)
    {
        IQueryable<WorkingStyle> queryWorkingStyle = _workingStyleRepository.Table.Include(c => c.JobPosts).Include(c => c.ProfileApplicants);
        queryWorkingStyle = queryWorkingStyle.GetWithSearch(searchWorkingStyleModel);
        // Apply sort
        queryWorkingStyle = queryWorkingStyle.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryWorkingStyle = queryWorkingStyle.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetWorkingStyleDetail>(queryWorkingStyle);
        return result.ToList();
    }

    public async Task<GetWorkingStyleDetail> GetWorkingStyleById(Guid id)
    {
        WorkingStyle workingStyle = await _workingStyleRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetWorkingStyleDetail>(workingStyle);
        return result;
    }

    public async Task<GetWorkingStyleDetail> CreateWorkingStyleAsync(CreateWorkingStyleModel requestBody)
    {
        WorkingStyle workingStyle = _mapper.Map<WorkingStyle>(requestBody);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _workingStyleRepository.InsertAsync(workingStyle);
        await _workingStyleRepository.SaveChangesAsync();
        GetWorkingStyleDetail workingStyleDetail = _mapper.Map<GetWorkingStyleDetail>(workingStyle);
        return workingStyleDetail;
    }

    public async Task<GetWorkingStyleDetail> UpdateWorkingStyleAsync(Guid id, UpdateWorkingStyleModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        WorkingStyle workingStyle = await _workingStyleRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        workingStyle = _mapper.Map(requestBody, workingStyle);
        _workingStyleRepository.Update(workingStyle);
        await _workingStyleRepository.SaveChangesAsync();
        GetWorkingStyleDetail workingStyleDetail = _mapper.Map<GetWorkingStyleDetail>(workingStyle);
        return workingStyleDetail;
    }

    public async Task DeleteWorkingStyleAsync(Guid id)
    {
        WorkingStyle workingStyle = await _workingStyleRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _workingStyleRepository.Delete(workingStyle);
        await _workingStyleRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _workingStyleRepository.GetAll().CountAsync();
    }
    
}