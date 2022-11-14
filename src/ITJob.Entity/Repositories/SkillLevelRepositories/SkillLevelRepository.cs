using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.SkillLevelRepositories;

public class SkillLevelRepository : BaseRepository<SkillLevel>, ISkillLevelRepository
{
    public SkillLevelRepository(DbContext context) : base(context)
    {
        
    }

    public SkillLevelRepository(DbContext context, DbSet<SkillLevel> dbsetExist) : base(context, dbsetExist)
    {
        
    }
    
}