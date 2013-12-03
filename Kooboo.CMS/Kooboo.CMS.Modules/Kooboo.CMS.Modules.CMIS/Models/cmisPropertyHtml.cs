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
    public partial class cmisPropertyHtml
    {
        public override string stringValue
        {
            get
            {
                if (this.value.Length > 0)
                {
                    return string.Join(",", this.value.Select(it => it.ToString()));
                }
                return null;
            }
        }
    }
}
