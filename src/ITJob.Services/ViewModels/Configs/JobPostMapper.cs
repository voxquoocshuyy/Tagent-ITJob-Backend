using AutoMapper;
using ITJob.Services.ViewModels.JobPost;

namespace ITJob.Services.ViewModels.Configs;

public static class JobPostMapper
{
    public static void ConfigJobPost(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.JobPost, GetJobPostDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.JobPost, CreateJobPostModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.JobPost, UpdateJobPostModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.JobPost, JobPostDetailScore>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.JobPost, UpdateJobPostExpriredModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.JobPost, UpdateJobPostMoneyModel>().ReverseMap();
        configuration.CreateMap<GetJobPostDetail, JobPostDetailScore>().ReverseMap();
    }
}