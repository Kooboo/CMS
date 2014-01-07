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
using Kooboo.Globalization;

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
        #region .ctor
        public ManagerBase(TProvider provider)
        {
            this.Provider = provider;
        }
        public TProvider Provider
        {
            get;
            set;
        }
        #endregion

        #region All
        public abstract IEnumerable<T> All(Site site, string filterName);

        #endregion

        #region Get
        public abstract T Get(Site site, string uuid);
        #endregion

        #region Update
        public abstract void Update(Site site, T @new, T old);
        #endregion

        #region Add
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
        #endregion

        #region Remove
        public virtual void Remove(Site site, T item)
        {
            item.Site = site;

            if (Relations(item).Count() > 0)
            {
                throw new Exception(string.Format("'{0}' is being used".Localize(), item.UUID));
            }

            Provider.Remove(item);
        }
        #endregion

        #region Relations
        public virtual IEnumerable<RelationModel> Relations(T o)
        {
            return new RelationModel[0];
        }
        #endregion
    }
}
