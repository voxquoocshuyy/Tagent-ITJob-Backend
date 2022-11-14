using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.SystemWalletRepositories;

public class SystemWalletRepository: BaseRepository<SystemWallet>, ISystemWalletRepository
{
    public SystemWalletRepository(DbContext context) : base(context)
    {
        
    }

    public SystemWalletRepository(DbContext context, DbSet<SystemWallet> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}