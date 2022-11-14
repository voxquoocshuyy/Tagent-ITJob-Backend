using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.JobPostRepositories;

public class JobPostRepository : BaseRepository<JobPost>, IJobPostRepository
{
    public JobPostRepository(DbContext context) : base(context)
    {
        
    }

    public JobPostRepository(DbContext context, DbSet<JobPost> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}