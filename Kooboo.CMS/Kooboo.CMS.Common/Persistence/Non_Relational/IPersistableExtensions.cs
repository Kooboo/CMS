#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Common.Persistence.Non_Relational
{
    public static class IPersistableExtensions
    {
        public static T AsActual<T>(this T persistable)
           where T : IPersistable
        {
            if (persistable == null)
            {
                return persistable;
            }
            if (persistable.IsDummy)
            {
                var provider = EngineContext.Current.Resolve<IProvider<T>>();
                return provider.Get(persistable);

            }
            return persistable;
        }
    }
}
