using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.JobPositionRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.JobPosition;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.JobPositionServices;

public class JobPositionService : IJobPositionService
{
    private readonly IJobPositionRepository _jobPositionRepository;
    private readonly IMapper _mapper;

    public JobPositionService(IJobPositionRepository jobPositionRepository, IMapper mapper)
    {
        _jobPositionRepository = jobPositionRepository;
        _mapper = mapper;
    }
    public IList<GetJobPositionDetail> GetJobPositionPage(PagingParam<JobPositionEnum.JobPositionSort> paginationModel, SearchJobPositionModel searchJobPositionModel)
    {
        IQueryable<JobPosition> queryJobPosition = _jobPositionRepository.Table.Include(c => c.ProfileApplicants);
        queryJobPosition = queryJobPosition.GetWithSearch(searchJobPositionModel);
        // Apply sort
        queryJobPosition = queryJobPosition.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryJobPosition = queryJobPosition.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetJobPositionDetail>(queryJobPosition);
        return result.ToList();
    }

    public async Task<GetJobPositionDetail> GetJobPositionById(Guid id)
    {
        JobPosition jobPosition = await _jobPositionRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (jobPosition == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetJobPositionDetail>(jobPosition);
        return result;
    }

    public async Task<GetJobPositionDetail> CreateJobPositionAsync(CreateJobPositionModel requestBody)
    {
        JobPosition jobPosition = _mapper.Map<JobPosition>(requestBody);
        if (jobPosition == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _jobPositionRepository.InsertAsync(jobPosition);
        await _jobPositionRepository.SaveChangesAsync();
        GetJobPositionDetail jobPositionDetail = _mapper.Map<GetJobPositionDetail>(jobPosition);
        return jobPositionDetail;
    }

    public async Task<GetJobPositionDetail> UpdateJobPositionAsync(Guid id, UpdateJobPositionModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        JobPosition jobPosition = await _jobPositionRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (jobPosition == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        jobPosition = _mapper.Map(requestBody, jobPosition);
        _jobPositionRepository.Update(jobPosition);
        await _jobPositionRepository.SaveChangesAsync();
        GetJobPositionDetail jobPositionDetail = _mapper.Map<GetJobPositionDetail>(jobPosition);
        return jobPositionDetail;
    }

    public async Task DeleteJobPositionAsync(Guid id)
    {
        JobPosition? jobPosition = await _jobPositionRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (jobPosition == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _jobPositionRepository.Delete(jobPosition);
        await _jobPositionRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _jobPositionRepository.GetAll().CountAsync();
    }
}