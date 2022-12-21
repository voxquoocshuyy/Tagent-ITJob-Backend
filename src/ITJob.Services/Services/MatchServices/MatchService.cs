using AutoMapper;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.BlockRepositories;
using ITJob.Entity.Repositories.CompanyRepositories;
using ITJob.Entity.Repositories.JobPostRepositories;
using ITJob.Entity.Repositories.LikeRepositories;
using ITJob.Entity.Repositories.ProfileApplicantRepositories;
using ITJob.Entity.Repositories.WorkingStyleRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.JobPost;
using ITJob.Services.ViewModels.ProfileApplicant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ITJob.Services.Services.MatchServices;

public class MatchService : IMatchService
{
    private readonly float _scoreDefault;
    private readonly float _scoreSkill;
    private readonly float _scoreWorkingStyle;
    private readonly float _scoreJobPosition;
    private readonly IMapper _mapper;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IProfileApplicantRepository _profileApplicantRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IBlockRepository _blockRepository;
    private readonly IApplicantRepository _applicantRepository;
    private readonly IWorkingStyleRepository _workingStyleRepository;

    public MatchService(IMapper mapper, IJobPostRepository jobPostRepository, IProfileApplicantRepository profileApplicantRepository,
        ILikeRepository likeRepository, IConfiguration config, ICompanyRepository companyRepository, IBlockRepository blockRepository, IApplicantRepository applicantRepository, IWorkingStyleRepository workingStyleRepository)
    {
        _mapper = mapper;
        _jobPostRepository = jobPostRepository;
        _profileApplicantRepository = profileApplicantRepository;
        _likeRepository = likeRepository;
        _scoreDefault = float.Parse(config["SystemConfiguration:ScoreDefault"]);
        _scoreWorkingStyle = float.Parse(config["SystemConfiguration:ScoreWorkingStyle"]);
        _scoreSkill = float.Parse(config["SystemConfiguration:ScoreSkill"]);
        _scoreJobPosition = float.Parse(config["SystemConfiguration:ScoreJobPosition"]);
        _companyRepository = companyRepository;
        _blockRepository = blockRepository;
        _applicantRepository = applicantRepository;
        _workingStyleRepository = workingStyleRepository;
    }

    // SUPPORT FUNCTION ----------
    private async Task<IQueryable<GetJobPostDetail>> CalculatorTotalScoreForProfileApplicant(Guid profileApplicantId)
    {
        var remote = _workingStyleRepository.Get(w => w.Name == "Remote")
            .Select(w => w.Id).FirstOrDefault();
        var freelance = _workingStyleRepository.Get(w => w.Name == "Freelance")
            .Select(w => w.Id).FirstOrDefault();
        var profileApplicant = await _profileApplicantRepository.Get( pa => pa.Id == profileApplicantId)
            .Include(pa => pa.ProfileApplicantSkills)
            .Include(pa => pa.Applicant).FirstOrDefaultAsync();

        var listSkillOfApplicant = profileApplicant!.ProfileApplicantSkills
            .Select(s => s.SkillId);
        var queryJobPostDetail =
            _jobPostRepository.GetAll().Include(jp => jp.JobPostSkills)
                .Where(jp => jp.Status == 0);
        
        var listJobPostScore =  _mapper.ProjectTo<JobPostDetailScore>(queryJobPostDetail).ToList();
        var locationOfApplicant = profileApplicant.Applicant?.Address;
        var splitOfProfileApplicant = locationOfApplicant?.Split(", ");
        var cityOfProfileApplicant = splitOfProfileApplicant?[1];
        foreach (var jobPost in listJobPostScore)
        {
            var locationOfJobPost = jobPost.WorkingPlace;
            var splitOfJobPost = locationOfJobPost?.Split(", ");
            var cityOfJobPost = splitOfJobPost?[1];
            var score = _scoreDefault;
            if ((jobPost.WorkingStyleId != remote || jobPost.WorkingStyleId != freelance) &&
                cityOfProfileApplicant != cityOfJobPost)
            {
                jobPost.Score = _scoreDefault;
            }
            else if((jobPost.WorkingStyleId == remote || jobPost.WorkingStyleId == freelance) &&
                    cityOfProfileApplicant != cityOfJobPost)
            {
                if (jobPost.WorkingStyleId == profileApplicant.WorkingStyleId) score += _scoreWorkingStyle;
                if (jobPost.JobPositionId == profileApplicant.JobPositionId) score += _scoreJobPosition;
                float countSkill = jobPost.JobPostSkills.Count(jp => listSkillOfApplicant.Contains(jp.SkillId));
                if(countSkill != 0)
                    score+= (countSkill / jobPost.JobPostSkills.Count)*_scoreSkill;
                jobPost.Score = score;
            }
            else if(cityOfProfileApplicant == cityOfJobPost)
            {
                if (jobPost.WorkingStyleId == profileApplicant.WorkingStyleId) score += _scoreWorkingStyle;
                if (jobPost.JobPositionId == profileApplicant.JobPositionId) score += _scoreJobPosition;
                float countSkill = jobPost.JobPostSkills.Count(jp => listSkillOfApplicant.Contains(jp.SkillId));
                if(countSkill != 0)
                    score+= (countSkill / jobPost.JobPostSkills.Count)*_scoreSkill;
                jobPost.Score = score;
            }
        }
        var listSorted = listJobPostScore.OrderBy(jp => jp.Score).AsQueryable(); 
        
        var jobPostDetail = _mapper.ProjectTo<GetJobPostDetail>(listSorted);

        return jobPostDetail;
    }
    
    private async Task<IQueryable<GetProfileApplicantDetail>> CalculatorTotalScoreForJobPost(Guid jobPostId)
    {
        var remote = _workingStyleRepository.Get(w => w.Name == "Remote")
            .Select(w => w.Id).FirstOrDefault();
        var freelance = _workingStyleRepository.Get(w => w.Name == "Freelance")
            .Select(w => w.Id).FirstOrDefault();
        var jobPost = await _jobPostRepository.Get( pa => pa.Id == jobPostId)
            .Include(pa => pa.JobPostSkills).FirstOrDefaultAsync();

        var listSkillOfJobPost = jobPost!.JobPostSkills.Select(s => s.SkillId).ToList();
        var queryProfileApplicantDetailAll =
            _profileApplicantRepository.GetAll()
                .Include(pa => pa.Applicant)
                .Include(pa => pa.ProfileApplicantSkills)
                .Where(pa => pa.Status == 0).ToList();
        
        var listProfileApplicantScore = _mapper.ProjectTo<ProfileApplicantScore>(queryProfileApplicantDetailAll.AsQueryable()).ToList();
        
        var locationOfJobPost = jobPost.WorkingPlace;
        var splitOfJobPost = locationOfJobPost?.Split(", ");
        var cityOfJobPost = splitOfJobPost?[1];
        
        foreach (var profileApplicant in listProfileApplicantScore)
        {
            var locationOfProfileApplicant = profileApplicant.Applicant?.Address;
            var splitOfProfileApplicant = locationOfProfileApplicant?.Split(", ");
            var cityOfProfileApplicant = splitOfProfileApplicant?[1];

            var score = _scoreDefault;
            
            if ((jobPost.WorkingStyleId != remote || jobPost.WorkingStyleId != freelance) &&
                cityOfProfileApplicant != cityOfJobPost)
            {
                profileApplicant.Score = _scoreDefault;
            }
            else if ((jobPost.WorkingStyleId == remote || jobPost.WorkingStyleId == freelance) &&
                     cityOfProfileApplicant != cityOfJobPost)
            {
                if (jobPost.WorkingStyleId == profileApplicant.WorkingStyleId) score += _scoreWorkingStyle;
                if (jobPost.JobPositionId == profileApplicant.JobPositionId) score += _scoreJobPosition;
                float countSkill =
                    profileApplicant.ProfileApplicantSkills.Count(pa => listSkillOfJobPost.Contains(pa.SkillId));
                if (countSkill != 0)
                    score += (countSkill / jobPost.JobPostSkills.Count) * _scoreSkill;
                profileApplicant.Score = score;
            }
            else if (cityOfProfileApplicant == cityOfJobPost)
            {
                if (jobPost.WorkingStyleId == profileApplicant.WorkingStyleId) score += _scoreWorkingStyle;
                if (jobPost.JobPositionId == profileApplicant.JobPositionId) score += _scoreJobPosition;
                float countSkill =
                    profileApplicant.ProfileApplicantSkills.Count(pa => listSkillOfJobPost.Contains(pa.SkillId));
                if (countSkill != 0)
                    score += (countSkill / jobPost.JobPostSkills.Count) * _scoreSkill;
                profileApplicant.Score = score;
            }
        }
        var listSorted = listProfileApplicantScore.OrderByDescending(pa => pa.Score)
            .AsQueryable();
        var profileApplicantDetail = _mapper.ProjectTo<GetProfileApplicantDetail>(listSorted);
        return profileApplicantDetail;
    }

    public async Task<IQueryable<GetJobPostDetail>> CalculatorTotalScoreForProfileApplicantFilter(Guid profileApplicantId,
        PagingParam<JobPostEnum.JobPostSort> paginationModel, SearchJobPostModel searchJobPostModel)
    {
        var profileApplicant =
            await _profileApplicantRepository.GetFirstOrDefaultAsync(pa => pa.Id == profileApplicantId);
        var applicant = await _applicantRepository.GetFirstOrDefaultAsync(a => a.Id == profileApplicant.ApplicantId);
        //get all
        var listJobPost = await CalculatorTotalScoreForProfileApplicant(profileApplicantId);
        //get like
        var queryProfileApplicantLike = _likeRepository.Get(l => l.ProfileApplicantId == profileApplicantId
                                                                 && l.IsProfileApplicantLike == true)
                                                                .Select(l => l.JobPostId).ToList();
        //get match
        var queryProfileApplicantMatch = _likeRepository.Get(l => l.ProfileApplicantId == profileApplicantId
                                                                 && l.Match == true)
                                                                .Select(l => l.JobPostId).ToList();
        //get block
        var listCompanyIdBlock = _blockRepository.Get(b => b.ApplicantId == applicant.Id)
            .Select(b => b.CompanyId).ToList();
        // apply except like
        var query = listJobPost.Where(jp => !queryProfileApplicantLike.Contains(jp.Id)
        && !queryProfileApplicantMatch.Contains(jp.Id));
        // apply except block
        query = query.Where(jp => !listCompanyIdBlock.Contains(jp.CompanyId));
        
        // query = query.GetWithSearch(searchJobPostModel);
        // Apply sort
        // query = query.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        
        // Apply Paging
        query.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        return query;

    }
    
    public async Task<IQueryable<GetProfileApplicantDetail>> CalculatorTotalScoreForJobPostFilter(Guid jobPostId,
        PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, SearchProfileApplicantModel searchProfileApplicantModel)
    {
        var jobPost = await _jobPostRepository.GetFirstOrDefaultAsync(jp => jp.Id == jobPostId);
        var company = await _companyRepository.Get(c => c.Id == jobPost.CompanyId).FirstOrDefaultAsync();
        // get all
        var listProfileApplicant = await CalculatorTotalScoreForJobPost(jobPostId);
        
        // get like
        var lisApplicantLikeJobPost = _likeRepository.Get(l => l.JobPostId == jobPostId
                                                                 && l.IsJobPostLike == true).Select(l => l.ProfileApplicantId).ToList();
        // get match
        var lisApplicantMatchJobPost = _likeRepository.Get(l => l.JobPostId == jobPostId
                                                               && l.Match == true).Select(l => l.ProfileApplicantId).ToList();
        
        // get block
        var listApplicantIdBlock = _blockRepository.Get(b => b.CompanyId == company.Id)
            .Select(b => b.ApplicantId).ToList();
        
        // apply except like
        var query = listProfileApplicant.Where(pa => !lisApplicantLikeJobPost.Contains(pa.Id) &&
            !lisApplicantMatchJobPost.Contains(pa.Id));
        // apply except block
        query = query.Where(pa => !listApplicantIdBlock.Contains(pa.ApplicantId));
        
        // query = query.GetWithSearch(searchProfileApplicantModel);
        // // Apply sort
        // query = query.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        
        // Apply Paging
        query = query.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        return query;
    }
    // public async Task<IQueryable<GetProfileApplicantDetail>> CalculatorTotalScoreForJobPostFilterLike(Guid jobPostId,
    //     PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, SearchProfileApplicantModel searchProfileApplicantModel)
    // {
    //     var jobPost = await _jobPostRepository.GetFirstOrDefaultAsync(jp => jp.Id == jobPostId);
    //     var company = await _companyRepository.Get(c => c.Id == jobPost.CompanyId).FirstOrDefaultAsync();
    //     // get all
    //     var listProfileApplicant = await CalculatorTotalScoreForJobPost(jobPostId);
    //     
    //     // get like
    //     var lisApplicantLikeJobPost = _likeRepository.Get(l => l.JobPostId == jobPostId
    //                                                            && l.IsJobPostLike == true).Select(l => l.ProfileApplicantId).ToList();
    //     
    //     // get block
    //     var listApplicantIdBlock = _blockRepository.Get(b => b.CompanyId == company.Id)
    //         .Select(b => b.ApplicantId).ToList();
    //     
    //     // apply except like
    //     var query = listProfileApplicant.Where(pa => lisApplicantLikeJobPost.Contains(pa.Id));
    //     
    //     // apply except block
    //     query = query.Where(pa => !listApplicantIdBlock.Contains(pa.ApplicantId));
    //     
    //     // query = query.GetWithSearch(searchProfileApplicantModel);
    //     // // Apply sort
    //     // query = query.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
    //     
    //     // Apply Paging
    //     query = query.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
    //     return query;
    // }
}