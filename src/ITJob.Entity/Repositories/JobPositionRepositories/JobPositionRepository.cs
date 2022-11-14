using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.JobPositionRepositories;

public class JobPositionRepository : BaseRepository<JobPosition>, IJobPositionRepository
{
    public JobPositionRepository(DbContext context) : base(context)
    {
        
    }

    public JobPositionRepository(DbContext context, DbSet<JobPosition> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}