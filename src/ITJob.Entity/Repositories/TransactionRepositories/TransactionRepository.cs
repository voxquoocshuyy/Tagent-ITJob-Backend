using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.TransactionRepositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(DbContext context) : base(context)
    {
        
    }

    public TransactionRepository(DbContext context, DbSet<Transaction> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}