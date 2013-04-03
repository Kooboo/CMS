using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Persistence;

namespace Kooboo.CMS.Account.Models
{
    public static class IPersistableExtensions
    {
        public static T AsActual<T>(this T persistable)
            where T : IPersistable
        {
            if (persistable.IsDummy)
            {
                persistable = Persistence.RepositoryFactory.Factory.GetRepository<IRepository<T>>().Get(persistable);
            }
            return persistable;
        }
    }
}
