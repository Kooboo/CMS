#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.SiteKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Persistence
{
    public interface IPageProvider : IProvider<Page>, IImportExportProvider<Page>
    {
        IEnumerable<Page> All(Site site);

        IEnumerable<Page> ChildPages(Page parentPage);

        Page GetDraft(Page page);

        void SaveAsDraft(Page page);

        void RemoveDraft(Page page);

    }
}
