#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Services
{
    public interface ILayoutService : IPersistenceService<Layout>, IImportExportService<Layout>, IInheritable<Layout>, IRelationService
    {
        IEnumerable<Layout> All(Site site);
    }
}
