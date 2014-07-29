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
    public interface ILabelService : IPersistenceService<Label>, IImportExportService<View>
    {
        IEnumerable<Label> All(Site site);

        IEnumerable<Label> All(Site site, string category);

        Label GetByUUID(Site site, string uuid);

        Label GetByName(Site site, string category, string name);
    }
}
