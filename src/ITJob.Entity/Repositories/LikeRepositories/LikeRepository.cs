using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.LikeRepositories;

public class LikeRepository : BaseRepository<Like>, ILikeRepository
{
    public LikeRepository(DbContext context) : base(context)
    {
        
    }

    public LikeRepository(DbContext context, DbSet<Like> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}