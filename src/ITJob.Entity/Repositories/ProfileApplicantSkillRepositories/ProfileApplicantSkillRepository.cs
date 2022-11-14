using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.ProfileApplicantSkillRepositories;

public class ProfileApplicantSkillRepository : BaseRepository<ProfileApplicantSkill>, IProfileApplicantSkillRepository
{
    public ProfileApplicantSkillRepository(DbContext context) : base(context)
    {
        
    }

    public ProfileApplicantSkillRepository(DbContext context, DbSet<ProfileApplicantSkill> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}