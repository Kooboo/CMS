#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Sites.Models
{
    public static class PageExtensions
    {
        public static Layout GetLayout(this Page page)
        {
            return (new Layout(page.Site, page.AsActual().Layout)).LastVersion();
        }
    }
}
