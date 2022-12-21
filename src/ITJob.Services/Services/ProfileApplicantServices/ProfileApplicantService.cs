using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.LikeRepositories;
using ITJob.Entity.Repositories.ProfileApplicantRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.ProfileApplicant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.ProfileApplicantServices;

public class ProfileApplicantService : IProfileApplicantService
{
    private readonly IProfileApplicantRepository _profileApplicantRepository;
    private readonly IMapper _mapper;
    private readonly ILikeRepository _likeRepository;

    public ProfileApplicantService(IProfileApplicantRepository profileApplicantRepository, IMapper mapper, ILikeRepository likeRepository)
    {
        _profileApplicantRepository = profileApplicantRepository;
        _mapper = mapper;
        _likeRepository = likeRepository;
    }

    public IList<GetProfileApplicantDetail> GetProfileApplicantPage(PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, SearchProfileApplicantModel searchProfileApplicantModel)
    {
        IQueryable<ProfileApplicant> queryProfileApplicant = _profileApplicantRepository.Table
            .Include(c => c.Applicant).Where(pa => pa.Status == 0 || pa.Status == 1)
            .Include(pa => pa.Certificates)
            // .Include(pa => pa.Likes)
            .Include(pa => pa.Projects)
            .Include(pa => pa.AlbumImages)
            .Include(pa => pa.WorkingExperiences)
            .Include(pa => pa.ProfileApplicantSkills)
            .Include(pa => pa.WorkingStyle)
            .Include(pa => pa.JobPosition);
        queryProfileApplicant = queryProfileApplicant.GetWithSearch(searchProfileApplicantModel);
        // Apply sort
        queryProfileApplicant = queryProfileApplicant.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryProfileApplicant = queryProfileApplicant.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetProfileApplicantDetail>(queryProfileApplicant);
        return result.ToList();
    }
    public async Task<GetProfileApplicantDetail> GetProfileApplicantById(Guid id)
    {
        ProfileApplicant profileApplicant = await _profileApplicantRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (profileApplicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetProfileApplicantDetail>(profileApplicant);
        return result;
    }

    public IList<GetProfileApplicantDetail> GetProfileApplicantLikePage(
        PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, 
        SearchProfileApplicantModel searchProfileApplicantModel, Guid jobPostId)
    {
        IQueryable<ProfileApplicant?> queryProfileApplicant =
            _likeRepository.Get(l => l.JobPostId == jobPostId)
                .Where(l => l.IsProfileApplicantLike == true && l.Match == null)
                .Include(p =>
                p.ProfileApplicant).Select(p => p.ProfileApplicant);
        queryProfileApplicant = queryProfileApplicant.GetWithSearch(searchProfileApplicantModel);
        // Apply sort
        queryProfileApplicant = queryProfileApplicant.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryProfileApplicant = queryProfileApplicant.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetProfileApplicantDetail>(queryProfileApplicant);
        return result.ToList();
    }
    public IList<GetProfileApplicantDetail> GetProfileApplicantJobPostLikePage(
        PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, 
        SearchProfileApplicantModel searchProfileApplicantModel, Guid jobPostId)
    {
        IQueryable<ProfileApplicant?> queryProfileApplicant =
            _likeRepository.Get(l => l.JobPostId == jobPostId)
                .Where(l => l.IsJobPostLike == true && l.Match == null)
                .Include(p =>
                    p.ProfileApplicant).Select(p => p.ProfileApplicant);
        queryProfileApplicant = queryProfileApplicant.GetWithSearch(searchProfileApplicantModel);
        // Apply sort
        queryProfileApplicant = queryProfileApplicant.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryProfileApplicant = queryProfileApplicant.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetProfileApplicantDetail>(queryProfileApplicant);
        return result.ToList();
    }

    public async Task<GetProfileApplicantDetail> CreateProfileApplicantAsync(CreateProfileApplicantModel requestBody)
    {
        ProfileApplicant profileApplicant = _mapper.Map<ProfileApplicant>(requestBody);
        if (profileApplicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        profileApplicant.CountShare = 0;
        profileApplicant.CountLike = 0;
        await _profileApplicantRepository.InsertAsync(profileApplicant);
        await _profileApplicantRepository.SaveChangesAsync();
        GetProfileApplicantDetail profileApplicantDetail = _mapper.Map<GetProfileApplicantDetail>(profileApplicant);
        return profileApplicantDetail;
    }

    public async Task<GetProfileApplicantDetail> UpdateProfileApplicantAsync(Guid id, UpdateProfileApplicantModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        ProfileApplicant profileApplicant = await _profileApplicantRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (profileApplicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        profileApplicant = _mapper.Map(requestBody, profileApplicant);
        _profileApplicantRepository.Update(profileApplicant);
        await _profileApplicantRepository.SaveChangesAsync();
        GetProfileApplicantDetail profileApplicantDetail = _mapper.Map<GetProfileApplicantDetail>(profileApplicant);
        return profileApplicantDetail;
    }

    public async Task DeleteProfileApplicantAsync(Guid id)
    {
        ProfileApplicant? profileApplicant = await _profileApplicantRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (profileApplicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _profileApplicantRepository.Delete(profileApplicant);
        await _profileApplicantRepository.SaveChangesAsync();
    }

    public async Task ResetCount()
    {
        var query = _profileApplicantRepository.GetAll().ToList();
        foreach (var profileApplicant in query)
        {
            profileApplicant.CountLike = 0;
            profileApplicant.CountShare = 0;
            _profileApplicantRepository.Update(profileApplicant);
            Console.WriteLine($"Reset: #{profileApplicant.Id} - {DateTime.Now}",
                Console.BackgroundColor == ConsoleColor.Yellow);
        }
        await _profileApplicantRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _profileApplicantRepository.GetAll().CountAsync();
    }
}