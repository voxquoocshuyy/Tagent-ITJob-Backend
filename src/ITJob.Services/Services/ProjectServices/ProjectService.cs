using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ProjectRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Project;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.ProjectServices;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    public IList<GetProjectDetail> GetProjectPage(PagingParam<ProjectEnum.ProjectSort> paginationModel, SearchProjectModel searchProjectModel)
    {
        IQueryable<Project> queryProject = _projectRepository.Table.Include(c => c.ProfileApplicant);
        queryProject = queryProject.GetWithSearch(searchProjectModel);
        // Apply sort
        queryProject = queryProject.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryProject = queryProject.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetProjectDetail>(queryProject);
        return result.ToList();
    }

    public async Task<GetProjectDetail> GetProjectById(Guid id)
    {
        Project project = await _projectRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (project == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetProjectDetail>(project);
        return result;
    }

    public async Task<GetProjectDetail> CreateProjectAsync(CreateProjectModel requestBody)
    {
        Project project = _mapper.Map<Project>(requestBody);
        if (project == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _projectRepository.InsertAsync(project);
        await _projectRepository.SaveChangesAsync();
        GetProjectDetail projectDetail = _mapper.Map<GetProjectDetail>(project);
        return projectDetail;
    }

    public async Task<GetProjectDetail> UpdateProjectAsync(Guid id, UpdateProjectModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Project project = await _projectRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (project == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        project = _mapper.Map(requestBody, project);
        _projectRepository.Update(project);
        await _projectRepository.SaveChangesAsync();
        GetProjectDetail projectDetail = _mapper.Map<GetProjectDetail>(project);
        return projectDetail;
    }

    public async Task DeleteProjectAsync(Guid id)
    {
        Project? project = await _projectRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (project == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _projectRepository.Delete(project);
        await _projectRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _projectRepository.GetAll().CountAsync();
    }
}