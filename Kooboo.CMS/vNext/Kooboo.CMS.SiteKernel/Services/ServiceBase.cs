#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.Common.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Services
{
    public abstract class ServiceBase<T, TProvider> : IPersistenceService<T>
        where T : ISiteObject, IPersistable, IIdentifiable
        where TProvider : IProvider<T>
    {
        #region .ctor
        public ServiceBase(TProvider provider)
        {
            this.Provider = provider;
        }
        public TProvider Provider
        {
            get;
            set;
        }
        #endregion

        #region Get
        public virtual T Get(T item)
        {
            return Provider.Get(item);
        }
        #endregion

        #region Add
        public virtual void Add(T item)
        {
            var o = Get(item);
            if (o != null)
            {
                throw new Exception(string.Format("Item '{0}' already exists.".Localize(), item.UUID));
            }
            Provider.Add(item);
        }
        #endregion

        #region Update
        public virtual void Update(T @new, T old)
        {
            Provider.Update(@new, old);
        }
        #endregion

        #region Remove
        public virtual void Remove(T item)
        {
            if (GetRelations(item).Count() > 0)
            {
                throw new Exception(string.Format("'{0}' is being used".Localize(), item.UUID));
            }

            Provider.Remove(item);
        }
        #endregion

        #region Relations
        public virtual IEnumerable<RelationModel> GetRelations<T>(T o)
        {
            return new RelationModel[0];
        }
        #endregion

        #region Import/Export
        public virtual void Import(Page data, System.IO.Stream zipData, bool @override)
        {
            throw new NotImplementedException();
        }

        public virtual System.IO.Stream[] Export(IEnumerable<Page> data)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
