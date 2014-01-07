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

namespace Kooboo.CMS.Sites.Extension.Management
{
    public class AssemblyReferenceCollection : List<AssemblyReferenceData>
    {
        public AssemblyReferenceData this[string name]
        {
            get
            {
                var data = this.Where(it => it.AssemblyName.EqualsOrNullEmpty(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                return data;
            }
        }
    }
}
