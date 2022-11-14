using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.RoleRepositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(DbContext context) : base(context)
    {
        
    }

    public RoleRepository(DbContext context, DbSet<Role> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}