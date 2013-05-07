#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Models;
using System.IO;
using Kooboo.Extensions;

namespace Kooboo.CMS.Sites.Services
{
    public interface IManager<T>
    {
        IEnumerable<T> All(Site site, string filterName);
        T Get(Site site, string name);
        void Update(Site site, T @new, T @old);
        void Add(Site site, T item);
        void Remove(Site site, T item);

        //void Export(Site site, string name, Stream outputStream);
        //void Import(Site site, string name, Stream zipStream, bool @override);
    }

    public abstract class PathResourceManagerBase<T, TProvider> : IManager<T>
        where T : PathResource
        where TProvider : ISiteElementProvider<T>
    {
        public PathResourceManagerBase(TProvider provider)
        {
            Provider = provider;
        }
        public TProvider Provider
        {
            get;
            set;
        }

        public virtual IEnumerable<T> All(Site site, string filterName)
        {
            var r = Provider.All(site);
            if (!string.IsNullOrEmpty(filterName))
            {
                r = r.Where(it => it.Name.Contains(filterName, StringComparison.CurrentCultureIgnoreCase));
            }

            return r;
        }

        public abstract T Get(Site site, string name);

        public virtual void Update(Site site, T @new, T @old)
        {
            if (string.IsNullOrEmpty(@new.Name))
            {
                throw new NameIsReqiredException();
            }

            old.Site = site;
            @new.Site = site;
            if (!old.Exists())
            {
                throw new ItemDoesNotExistException();
            }

            @new.LastUpdateDate = DateTime.UtcNow;

            Provider.Update(@new, @old);
        }

        public virtual void Add(Site site, T o)
        {
            if (string.IsNullOrEmpty(o.Name))
            {
                throw new NameIsReqiredException();
            }

            o.Site = site;
            if (o.Exists())
            {
                throw new ItemAlreadyExistsException();
            }

            o.LastUpdateDate = DateTime.UtcNow;

            Provider.Add(o);
        }

        public virtual void Remove(Site site, T o)
        {
            //if (string.IsNullOrEmpty(o.Name))
            //{
            //    throw new NameIsReqiredException();
            //}

            o.Site = site;
            //if (!o.Exists())
            //{
            //    throw new ItemDoesNotExistException();
            //}
            if (o.Exists())
            {
                Provider.Remove(o);
            }

        }


    }
}
