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
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Services
{
    public interface IPersistenceService<T>
    {
        T Get(T o);
        void Add(T o);
        void Update(T @new, T @old);
        void Delete(T o);
    }
}
