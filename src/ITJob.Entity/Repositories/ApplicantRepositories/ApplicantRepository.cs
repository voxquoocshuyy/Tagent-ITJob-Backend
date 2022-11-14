using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.ApplicantRepositories;

public class ApplicantRepository : BaseRepository<Applicant>, IApplicantRepository
{
    public ApplicantRepository(DbContext context) : base(context)
    {
        
    }
    public ApplicantRepository(DbContext context, DbSet<Applicant> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}