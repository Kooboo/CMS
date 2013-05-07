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
using Kooboo.Web.Mvc.Paging;

namespace Kooboo.CMS.Sites.DataRule
{
    public class DataRulePagedList : PagedList<IDictionary<string, object>>
    {
        public DataRulePagedList(IEnumerable<IDictionary<string, object>> items, int pageIndex, int pageSize, int totalItemCount)
            : base(items, pageIndex, pageSize, totalItemCount)
        {
            PageIndexParameterName = "PageIndex";
        }
        public string PageIndexParameterName { get; set; }
    }
}
