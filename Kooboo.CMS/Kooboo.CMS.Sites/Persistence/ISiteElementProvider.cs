#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface ISiteElementProvider<T> : IProvider<T>
    {
        IEnumerable<T> All(Site site);
        
        //void InitializeToDB(Site site);

        //void ExportToDisk(Site site);
    }
}
