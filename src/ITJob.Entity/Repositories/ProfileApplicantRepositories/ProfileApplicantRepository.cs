using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.ProfileApplicantRepositories;

public class ProfileApplicantRepository : BaseRepository<ProfileApplicant>, IProfileApplicantRepository
{
    public ProfileApplicantRepository(DbContext context) : base(context)
    {
        
    }

    public ProfileApplicantRepository(DbContext context, DbSet<ProfileApplicant> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}