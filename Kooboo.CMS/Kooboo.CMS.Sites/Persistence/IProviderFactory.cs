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
using System.ComponentModel.Composition;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IProviderFactory
    {
        T GetProvider<T>()
            where T : class;

        void RegisterProvider<ServiceType>(ServiceType provider)
            where ServiceType : class;
    }
}
