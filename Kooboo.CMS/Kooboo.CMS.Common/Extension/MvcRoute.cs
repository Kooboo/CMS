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

namespace Kooboo.CMS.Common.Extension
{
    public class MvcRoute
    {
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public IDictionary<string, object> RouteValues { get; set; }
    }
}
