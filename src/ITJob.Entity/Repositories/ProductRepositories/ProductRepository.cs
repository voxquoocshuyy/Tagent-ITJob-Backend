using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Entity.Repositories.ProductRepositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(DbContext context) : base(context)
    {
        
    }

    public ProductRepository(DbContext context, DbSet<Product> dbsetExist) : base(context, dbsetExist)
    {
        
    }
}