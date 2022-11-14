using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.AlbumImageRepositories;

public class AlbumImageRepository : BaseRepository<AlbumImage>, IAlbumImageRepository
{
    public AlbumImageRepository(DbContext context) : base(context)
    {
        
    }
    public AlbumImageRepository(DbContext context, DbSet<AlbumImage> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}