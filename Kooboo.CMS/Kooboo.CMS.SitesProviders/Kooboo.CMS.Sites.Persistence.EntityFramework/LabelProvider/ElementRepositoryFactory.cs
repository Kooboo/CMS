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
using Kooboo.CMS.Sites.Globalization;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.LabelProvider
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IElementRepositoryFactory), Order = 100)]
    public class ElementRepositoryFactory : IElementRepositoryFactory
    {
        #region .ctor
        SiteDBContext _dbContext;
        public ElementRepositoryFactory(SiteDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        public Kooboo.Common.Globalization.IElementRepository CreateRepository(Models.Site site)
        {
            return new LabelRepository(site.FullName, _dbContext);
        }
    }
}
