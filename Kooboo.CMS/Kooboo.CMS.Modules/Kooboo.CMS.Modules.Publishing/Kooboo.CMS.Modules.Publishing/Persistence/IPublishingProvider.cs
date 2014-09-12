#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence
{
    public interface IPublishingProvider<T> : Kooboo.CMS.Common.Persistence.Non_Relational.IProvider<T>
    {
        IEnumerable<T> All();
        IEnumerable<T> All(Site site);
    }
}
