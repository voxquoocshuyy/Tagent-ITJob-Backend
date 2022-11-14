using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.UserRepositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
        
    }

    public UserRepository(DbContext context, DbSet<User> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}