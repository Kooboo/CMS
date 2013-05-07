#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.eCommerce.EventBus;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Catalog
{
    [Dependency(typeof(ICategoryService))]
    public class CategoryService : RelationalServiceBase<Category, ICategoryProvider>, ICategoryService
    {
        #region .ctor
        public CategoryService(ICategoryProvider provider, IEventPublisher eventPublisher)
            : base(provider, eventPublisher)
        {
        }
        #endregion
    }
}
