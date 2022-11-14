using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.JobPostSkillRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.JobPostSkill;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.JobPostSkillServices;

public class JobPostSkillService : IJobPostSkillService
{
    private readonly IJobPostSkillRepository _jobPostSkillRepository;
    private readonly IMapper _mapper;

    public JobPostSkillService(IJobPostSkillRepository jobPostSkillRepository, IMapper mapper)
    {
        _jobPostSkillRepository = jobPostSkillRepository;
        _mapper = mapper;
    }
    public IList<GetJobPostSkillDetail> GetJobPostSkillPage(PagingParam<JobPostSkillEnum.JobPostSkillSort> paginationModel, SearchJobPostSkillModel searchJobPostSkillModel)
    {
        IQueryable<JobPostSkill> queryJobPostSkill = _jobPostSkillRepository.Table.Include(c => c.JobPost).Include(c => c.Skill);
        queryJobPostSkill = queryJobPostSkill.GetWithSearch(searchJobPostSkillModel);
        // Apply sort
        queryJobPostSkill = queryJobPostSkill.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryJobPostSkill = queryJobPostSkill.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetJobPostSkillDetail>(queryJobPostSkill);
        return result.ToList();
    }

    public async Task<GetJobPostSkillDetail> GetJobPostSkillById(Guid id)
    {
        JobPostSkill jobPostSkill = await _jobPostSkillRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (jobPostSkill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetJobPostSkillDetail>(jobPostSkill);
        return result;
    }

    public async Task<GetJobPostSkillDetail> CreateJobPostSkillAsync(CreateJobPostSkillModel requestBody)
    {
        JobPostSkill jobPostSkill = _mapper.Map<JobPostSkill>(requestBody);
        if (jobPostSkill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _jobPostSkillRepository.InsertAsync(jobPostSkill);
        await _jobPostSkillRepository.SaveChangesAsync();
        GetJobPostSkillDetail jobPostSkillDetail = _mapper.Map<GetJobPostSkillDetail>(jobPostSkill);
        return jobPostSkillDetail;
    }

    public async Task<GetJobPostSkillDetail> UpdateJobPostSkillAsync(Guid id, UpdateJobPostSkillModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        JobPostSkill jobPostSkill = await _jobPostSkillRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (jobPostSkill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        jobPostSkill = _mapper.Map(requestBody, jobPostSkill);
        _jobPostSkillRepository.Update(jobPostSkill);
        await _jobPostSkillRepository.SaveChangesAsync();
        GetJobPostSkillDetail jobPostSkillDetail = _mapper.Map<GetJobPostSkillDetail>(jobPostSkill);
        return jobPostSkillDetail;
    }

    public async Task DeleteJobPostSkillAsync(Guid id)
    {
        JobPostSkill? jobPostSkill = await _jobPostSkillRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (jobPostSkill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _jobPostSkillRepository.Delete(jobPostSkill);
        await _jobPostSkillRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _jobPostSkillRepository.GetAll().CountAsync();
    }
}