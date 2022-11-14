using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.SkillGroupRepositories;

public class SkillGroupRepository : BaseRepository<SkillGroup>, ISkillGroupRepository
{
    public SkillGroupRepository(DbContext context) : base(context)
    {
        
    }

    public SkillGroupRepository(DbContext context, DbSet<SkillGroup> dbsetExist) : base(context, dbsetExist)
    {
        
    }

}