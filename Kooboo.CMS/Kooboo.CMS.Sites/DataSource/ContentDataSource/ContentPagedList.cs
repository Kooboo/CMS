#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource.ContentDataSource
{
    public class ContentPagedList : PagedList<IDictionary<string, object>>
    {
        public ContentPagedList(IEnumerable<IDictionary<string, object>> items, int pageIndex, int pageSize, int totalItemCount)
            : base(items, pageIndex, pageSize, totalItemCount)
        {
            PageIndexParameterName = "PageIndex";
        }
        public string PageIndexParameterName { get; set; }
    }
}
