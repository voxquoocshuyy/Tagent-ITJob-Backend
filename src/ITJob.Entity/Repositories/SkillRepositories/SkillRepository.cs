using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.SkillRepositories;

public class SkillRepository : BaseRepository<Skill>, ISkillRepository
{
    public SkillRepository(DbContext context) : base(context)
    {
        
    }

    public SkillRepository(DbContext context, DbSet<Skill> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}