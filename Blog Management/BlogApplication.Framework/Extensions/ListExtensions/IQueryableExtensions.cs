using System;
using System.Linq;

namespace BlogApplication.Framework.Extensions.ListExtensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TResult> Paginate<TResult>(
          this IQueryable<TResult> source, int page, int pageSize)
        {
            if (pageSize > 0 && page > 0)
            {
                int skip = Math.Max(pageSize * (page - 1), 0);
                return source.Skip(skip).Take(pageSize);
            }
            return source;
        }
    }
}