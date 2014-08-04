#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Persistence.FileSystem
{
    public class ProviderRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 1; }
        }

        public void Register(IContainerManager containerManager, Kooboo.Common.ObjectContainer.ITypeFinder typeFinder)
        {
            
        }
    }
}
