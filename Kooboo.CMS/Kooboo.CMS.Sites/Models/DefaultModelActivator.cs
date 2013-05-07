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

namespace Kooboo.CMS.Sites.Models
{
    public class DefaultModelActivator<T> : IModelActivator<T>
    {
        #region IModelActivator<T> Members

        public T Create(Site site, string name)
        {
            return (T)Activator.CreateInstance(typeof(T), site, name);
        }

        public T CreateDummy(Site site)
        {
            return (T)Activator.CreateInstance(typeof(T), site, "dummy");
        }

        public T Create(string physicalPath)
        {
            return (T)Activator.CreateInstance(typeof(T), physicalPath);
        }

        #endregion
    }
    public class DefaultCascadableModelActivator<T> : DefaultModelActivator<T>, ICascadableModelActivator<T>
    {
        #region ICascadableModelActivator<T> Members

        public T Create(T parent, string name)
        {
            return (T)Activator.CreateInstance(typeof(T), parent, name);
        }

        #endregion
    }
}
