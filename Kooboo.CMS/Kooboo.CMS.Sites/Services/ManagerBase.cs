#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Services
{
    public abstract class ManagerBase<T, TProvider> : IManager<T>
        where T : ISiteObject, IPersistable, IIdentifiable
        where TProvider : ISiteElementProvider<T>
    {
        public ManagerBase(TProvider provider)
        {
            this.Provider = provider;
        }
        public TProvider Provider
        {
            get;
            set;
        }

        public abstract IEnumerable<T> All(Site site, string filterName);

        public abstract T Get(Site site, string name);

        public abstract void Update(Site site, T @new, T old);

        public virtual void Add(Site site, T item)
        {
            item.Site = site;
            var o = item.AsActual();
            if (o != null)
            {
                throw new ItemAlreadyExistsException();
            }
            Provider.Add(item);
        }

        public virtual void Remove(Site site, T item)
        {
            item.Site = site;
            Provider.Remove(item);
        }
    }
}
