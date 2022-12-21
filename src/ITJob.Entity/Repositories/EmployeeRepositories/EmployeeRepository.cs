using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.EmployeeRepositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(DbContext context) : base(context)
    {
        
    }

    public EmployeeRepository(DbContext context, DbSet<Employee> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}