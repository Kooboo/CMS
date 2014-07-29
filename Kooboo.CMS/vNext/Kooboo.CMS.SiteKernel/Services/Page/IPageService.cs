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
    public interface PageService : IPersistenceService<Page>, IImportExportService<Page>, IInheritable<Page>
    {
        IEnumerable<Page> AllRootPages(Site site);
        IEnumerable<Page> ChildPages(Page parentPage);

        void Move(IEnumerable<Page> pages, Page newParent);

        IEnumerable<Page> GetUnsyncedPages(Site site, Page parentPage);
    }
}
