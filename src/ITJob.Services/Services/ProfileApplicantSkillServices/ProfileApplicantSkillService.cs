using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ProfileApplicantSkillRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.ProfileApplicantSkill;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.ProfileApplicantSkillServices;

public class ProfileApplicantSkillService : IProfileApplicantSkillService
{
    private readonly IProfileApplicantSkillRepository _profileApplicantSkillRepository;
    private readonly IMapper _mapper;

    public ProfileApplicantSkillService(IProfileApplicantSkillRepository profileApplicantSkillRepository, IMapper mapper)
    {
        _profileApplicantSkillRepository = profileApplicantSkillRepository;
        _mapper = mapper;
    }
    public IList<GetProfileApplicantSkillDetail> GetProfileApplicantSkillPage(PagingParam<ProfileApplicantSkillEnum.ProfileApplicantSkillSort> paginationModel, SearchProfileApplicantSkillModel searchProfileApplicantSkillModel)
    {
        IQueryable<ProfileApplicantSkill> queryProfileApplicantSkill = _profileApplicantSkillRepository.Table.Include(c => c.ProfileApplicant);
        queryProfileApplicantSkill = queryProfileApplicantSkill.GetWithSearch(searchProfileApplicantSkillModel);
        // Apply sort
        queryProfileApplicantSkill = queryProfileApplicantSkill.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryProfileApplicantSkill = queryProfileApplicantSkill.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetProfileApplicantSkillDetail>(queryProfileApplicantSkill);
        return result.ToList();
    }

    public async Task<GetProfileApplicantSkillDetail> GetProfileApplicantSkillById(Guid id)
    {
        ProfileApplicantSkill profileApplicantSkill = await _profileApplicantSkillRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (profileApplicantSkill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetProfileApplicantSkillDetail>(profileApplicantSkill);
        return result;
    }

    public async Task<GetProfileApplicantSkillDetail> CreateProfileApplicantSkillAsync(CreateProfileApplicantSkillModel requestBody)
    {
        ProfileApplicantSkill profileApplicantSkill = _mapper.Map<ProfileApplicantSkill>(requestBody);
        if (profileApplicantSkill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _profileApplicantSkillRepository.InsertAsync(profileApplicantSkill);
        await _profileApplicantSkillRepository.SaveChangesAsync();
        GetProfileApplicantSkillDetail profileApplicantSkillDetail = _mapper.Map<GetProfileApplicantSkillDetail>(profileApplicantSkill);
        return profileApplicantSkillDetail;
    }

    public async Task<GetProfileApplicantSkillDetail> UpdateProfileApplicantSkillAsync(Guid id, UpdateProfileApplicantSkillModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        ProfileApplicantSkill profileApplicantSkill = await _profileApplicantSkillRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (profileApplicantSkill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        profileApplicantSkill = _mapper.Map(requestBody, profileApplicantSkill);
        _profileApplicantSkillRepository.Update(profileApplicantSkill);
        await _profileApplicantSkillRepository.SaveChangesAsync();
        GetProfileApplicantSkillDetail profileApplicantSkillDetail = _mapper.Map<GetProfileApplicantSkillDetail>(profileApplicantSkill);
        return profileApplicantSkillDetail;
    }

    public async Task DeleteProfileApplicantSkillAsync(Guid id)
    {
        ProfileApplicantSkill? profileApplicantSkill = await _profileApplicantSkillRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (profileApplicantSkill == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _profileApplicantSkillRepository.Delete(profileApplicantSkill);
        await _profileApplicantSkillRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _profileApplicantSkillRepository.GetAll().CountAsync();
    }
}