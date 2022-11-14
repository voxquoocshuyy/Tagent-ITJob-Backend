using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.WorkingExperienceRepositories;

public class WorkingExperienceRepository : BaseRepository<WorkingExperience>, IWorkingExperienceRepository
{
    public WorkingExperienceRepository(DbContext context) : base(context)
    {
        
    }

    public WorkingExperienceRepository(DbContext context, DbSet<WorkingExperience> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}