#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Web.Grid.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [Grid()]
    public class ABPageTestHitsReport
    {
        [GridColumnAttribute(HeaderText = "Page name", GridColumnType = typeof(SortableGridColumn), Order = 1)]
        public string PageName { get; set; }
        [GridColumnAttribute(HeaderText = "Show times", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(ABPageTestHitsReport_ShowTimes_GridItemColumn), Order = 2)]
        public int ShowTimes { get; set; }
        [GridColumnAttribute(HeaderText = "Hit times", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(ABPageTestHitsReport_HitTimes_GridItemColumn), Order = 3)]
        public int HitTimes { get; set; }

        public double ShowRate { get; set; }

        public double HitRate { get; set; }
    }
}
