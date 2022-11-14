using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.TransactionJobPostRepositories;

public class TransactionJobPostRepository : BaseRepository<TransactionJobPost>, ITransactionJobPostRepository
{
    public TransactionJobPostRepository(DbContext context) : base(context)
    {
        
    }

    public TransactionJobPostRepository(DbContext context, DbSet<TransactionJobPost> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}