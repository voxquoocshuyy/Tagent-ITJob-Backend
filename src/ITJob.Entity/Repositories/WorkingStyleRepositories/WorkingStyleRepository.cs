using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.WorkingStyleRepositories;

public class WorkingStyleRepository : BaseRepository<WorkingStyle>, IWorkingStyleRepository
{
    public WorkingStyleRepository(DbContext context) : base(context)
    {
        
    }

    public WorkingStyleRepository(DbContext context, DbSet<WorkingStyle> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}