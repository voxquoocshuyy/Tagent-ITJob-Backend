using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.JobPostSkillRepositories;

public class JobPostSkillRepository : BaseRepository<JobPostSkill>, IJobPostSkillRepository
{
    public JobPostSkillRepository(DbContext context) : base(context)
    {
        
    }

    public JobPostSkillRepository(DbContext context, DbSet<JobPostSkill> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}