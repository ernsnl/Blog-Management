using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogApplication.Framework.Extensions.ListExtensions
{
    public static class IEnumarableExtensions
    {
        public static IEnumerable<TResult> Paginate<TResult>(this IEnumerable<TResult> source, int page, int pageSize)
        {
            if (page > 0 && pageSize > 0)
            {
                int skip = Math.Max(pageSize*(page - 1), 0);
                return source.Skip(skip).Take(pageSize);

            }
            return source;
        }

        public static IEnumerable<TResult> Where<TResult>(this IEnumerable<TResult> source, string property, object value) where TResult : class
        {
            return source.Where(item => item.GetPropertyValue(property).Equals(value));
        }
    }
}