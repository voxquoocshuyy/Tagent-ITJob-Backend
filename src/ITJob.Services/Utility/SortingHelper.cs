using System.Linq.Expressions;
using ITJob.Services.Utility.Paging;

namespace ITJob.Services.Utility;

public static class SortingHelper
{
    public static IQueryable<TObject> GetWithSorting<TObject>(this IQueryable<TObject> source,
        string sortKey, PagingConstant.OrderCriteria sortOrder) 
        where TObject : class
    {
        if (source == null) return Enumerable.Empty<TObject>().AsQueryable();

        if (sortKey != null)
        {
            var param = Expression.Parameter(typeof(TObject), "p");
            var prop = Expression.Property(param, sortKey);
            var exp = Expression.Lambda(prop, param);
            string method = "";
            switch (sortOrder)
            {
                case PagingConstant.OrderCriteria.ASC:
                    method = "OrderBy";
                    break;
                default :
                    method = "OrderByDescending";
                    break;
            }
            Type[] types = new Type[] { source.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, source.Expression, exp);
            return source.Provider.CreateQuery<TObject>(mce);
        }
        return source;
    }
}