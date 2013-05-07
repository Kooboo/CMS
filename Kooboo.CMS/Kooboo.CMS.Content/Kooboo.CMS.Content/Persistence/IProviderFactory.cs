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

namespace Kooboo.CMS.Content.Persistence
{
    public interface IProviderFactory
    {
        string Name { get; }

        T GetProvider<T>()
            where T : class;

        void RegisterProvider<ServiceType>(ServiceType provider)
            where ServiceType : class;
    }
}
