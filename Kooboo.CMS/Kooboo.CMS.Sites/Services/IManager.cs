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

namespace Kooboo.CMS.Sites.Services
{
    public interface IManager<T>
    {
        IEnumerable<T> All(Site site, string filterName);

        T Get(Site site, string uuid);

        void Update(Site site, T @new, T @old);

        void Add(Site site, T item);

        void Remove(Site site, T item);
    }
}
