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
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization.Repository;

namespace Kooboo.CMS.Sites.Globalization
{
    public interface IElementRepositoryFactory
    {
        IElementRepository CreateRepository(Site site);
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IElementRepositoryFactory))]
    public class DefaultElementRepositoryFactory : IElementRepositoryFactory
    {
        public IElementRepository CreateRepository(Site site)
        {
            return new SiteLabelRepository(site);
        }
    }
}
