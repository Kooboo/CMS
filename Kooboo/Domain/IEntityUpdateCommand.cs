using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Kooboo.Domain
{
    public interface IEntityUpdateCommand<TEntity> : IEntityCommand
    {
        IQueryable<TEntity> EntitySet
        {
            get;
            set;
        }

        IEntityUpdateCommand<TEntity> Set<TProperty>(Expression<Func<TEntity, TProperty>> field, TProperty value);       
            
    }
}
