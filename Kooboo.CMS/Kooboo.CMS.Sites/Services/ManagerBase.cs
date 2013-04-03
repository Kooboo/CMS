using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;

namespace Kooboo.CMS.Sites.Services
{
    public abstract class ManagerBase<T, TProvider> : IManager<T>
        where T : IPersistable
        where TProvider : IProvider<T>
    {
        public TProvider Provider
        {
            get { return Persistence.Providers.ProviderFactory.GetRepository<TProvider>(); }
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
