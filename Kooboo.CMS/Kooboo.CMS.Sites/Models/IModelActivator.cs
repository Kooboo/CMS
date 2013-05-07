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

namespace Kooboo.CMS.Sites.Models
{
    public interface IModelActivator<T>
    {
        T Create(Site site, string name);
        T CreateDummy(Site site);
        T Create(string physicalPath);
    }
    public interface ICascadableModelActivator<T> : IModelActivator<T>
    {
        T Create(T parent, string name);
    }
}
