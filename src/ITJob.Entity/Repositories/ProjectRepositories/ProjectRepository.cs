using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.ProjectRepositories;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(DbContext context) : base(context)
    {
        
    }

    public ProjectRepository(DbContext context, DbSet<Project> dbsetExist) : base(context, dbsetExist)
    {
        
    }
    
}