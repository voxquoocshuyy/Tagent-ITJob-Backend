using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.BlockRepositories;

public class BlockRepository : BaseRepository<Block>, IBlockRepository
{
    public BlockRepository(DbContext context) : base(context)
    {
        
    }
    public BlockRepository(DbContext context, DbSet<Block> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}