#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Services
{
    public interface IManagerBase<T>
    {
        void Add(T obj);
        void Update(T @new, T old);
        void Delete(T membership);
    }

    public abstract class ManagerBase<T> : IManagerBase<T>
    {
        #region .ctor
        IProvider<T> _provider;
        public ManagerBase(IProvider<T> provider)
        {
            _provider = provider;
        }
        #endregion

        #region Add
        public virtual void Add(T obj)
        {
            _provider.Add(obj);
        }
        #endregion

        #region Update
        public virtual void Update(T @new, T old)
        {
            _provider.Update(@new, old);
        }
        #endregion

        #region Delete
        public virtual void Delete(T obj)
        {
            _provider.Remove(obj);
        }
        #endregion
    }
}
