using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Kooboo.IoC;

namespace Kooboo.Domain
{
    public static class IQeuryableExtensions
    {
        public static IEntityUpdateCommand<TEntity> ToUpdate<TEntity>(this IQueryable<TEntity> query)
        {
        
            var command = ContextContainer.Current.Resolve<IEntityUpdateCommand<TEntity>>();
            command.EntitySet = query;

            return command;
        }

        public static IEntityUpdateCommand<TEntity> ToUpdate<TEntity>(this IEnumerable<TEntity> query)
        {
            return query.AsQueryable().ToUpdate();
        }

        public static IEntityDeleteCommand<TEntity> ToDelete<TEntity>(this IQueryable<TEntity> query)
        {
            var command = ContextContainer.Current.Resolve<IEntityDeleteCommand<TEntity>>();
            command.EntitySet = query;
            return command;
        }

        public static IEntityDeleteCommand<TEntity> ToDelete<TEntity>(this IEnumerable<TEntity> query)
        {
            return query.AsQueryable().ToDelete();
        }
    }
}
