using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.CertificateRepositories;

public class CertificateRepository : BaseRepository<Certificate>, ICertificateRepository
{
    public CertificateRepository(DbContext context) : base(context)
    {
        
    }

    public CertificateRepository(DbContext context, DbSet<Certificate> dbsetExist) : base(context, dbsetExist)
    {
        
    }
    
}