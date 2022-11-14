using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.WalletRepositories;

public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
{
    public WalletRepository(DbContext context) : base(context)
    {
        
    }

    public WalletRepository(DbContext context, DbSet<Wallet> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}