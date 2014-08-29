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

namespace Kooboo.Web.Mvc
{
    /// <summary>
    /// Menu item 文本上面的一些小标记
    /// </summary>
    public class Badge
    {
        public string Text { get; set; }
        public IDictionary<string, object> HtmlAttributes { get; set; }
    }
}
