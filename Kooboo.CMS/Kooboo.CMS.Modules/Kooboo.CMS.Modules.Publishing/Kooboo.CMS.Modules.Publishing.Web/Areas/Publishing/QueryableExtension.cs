using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing
{
    public static class QueryableExtension
    {
        public static IQueryable<T> SortByField<T>(this IQueryable<T> query, string fieldName, string sortDir)
            where T : class
        {
            var modelType = typeof(T);
            var prop = modelType.GetProperties().FirstOrDefault(it => it.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
            if (prop != null)
            {
                string sortMethodName = "OrderBy";
                if ("desc".Equals(sortDir, StringComparison.OrdinalIgnoreCase))
                {
                    sortMethodName = "OrderByDescending";
                }
                var parameter = Expression.Parameter(modelType, "it");
                var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                var resultExpression = Expression.Call(typeof(Queryable), sortMethodName, new Type[] { modelType, prop.PropertyType }, query.Expression, Expression.Quote(orderByExpression));
                query = query.Provider.CreateQuery<T>(resultExpression);
            }
            return query;
        }
    }
}