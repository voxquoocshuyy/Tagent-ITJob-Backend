using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.CompanyRepositories;

public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
{
    public CompanyRepository(DbContext context) : base(context)
    {
        
    }

    public CompanyRepository(DbContext context, DbSet<Company> dbsetExist) : base(context, dbsetExist)
    {
        
    }

}