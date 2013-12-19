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
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Mvc.WebResourceLoader.DynamicClientResource
{
    public class DynamicClientResourceFactory
    {
        public static DynamicClientResourceFactory Default = (DynamicClientResourceFactory)TypeActivator.CreateInstance(typeof(DynamicClientResourceFactory));

        private IList<IDynamicClientResource> providers = new List<IDynamicClientResource>();
        public virtual void RegisterDynamicCssProvider(IDynamicClientResource dynamicCss)
        {
            providers.Add(dynamicCss);
        }
        public virtual IEnumerable<IDynamicClientResource> ResolveAllProviders()
        {
            return providers;
        }
        public virtual IDynamicClientResource ResolveProvider(string filePath)
        {
            foreach (var item in ResolveAllProviders())
            {
                if (item.Accept(filePath))
                {
                    return item;
                }
            }
            return null;
        }
    }
}
