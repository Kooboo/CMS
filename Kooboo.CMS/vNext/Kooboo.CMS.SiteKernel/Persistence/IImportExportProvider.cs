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

namespace Kooboo.CMS.SiteKernel.Persistence
{
    public interface IImportExportProvider<T>
    {
        void Import(T data, byte[] zipData, IDictionary<string, object> options);

        byte[] Export(IEnumerable<T> data, IDictionary<string, object> options);
    }
}
