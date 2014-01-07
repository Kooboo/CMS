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

namespace Kooboo.CMS.Modules.CMIS.Models
{
    public partial class cmisPropertyString
    {
        public override string stringValue
        {
            get
            {
                if (this.value != null && this.value.Length > 0)
                {
                    return string.Join(",", this.value.Where(it => it != null).Select(it => it.ToString()));
                }
                return null;
            }
        }
    }
}
