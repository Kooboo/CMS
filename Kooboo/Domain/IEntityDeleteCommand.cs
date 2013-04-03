using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Kooboo.Domain
{
    public interface IEntityDeleteCommand<TEntity> : IEntityCommand
    {
        IQueryable<TEntity> EntitySet
        {
            get;
            set;
        }
    }
}
