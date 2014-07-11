using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Web.Grid.Design;
using Kooboo.Common.Web.Grid.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(ABPageTestResult))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class ABPageTestResult_Metadata
    {
        [GridColumnAttribute(HeaderText = "A/B test name", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(HitReport_GridItemColumn), Order = 1)]
        public string ABPageUUID { get; set; }
        public IList<ABPageTestHitsReport> PageHits { get; set; }
        [GridColumnAttribute(HeaderText = "Show times", GridColumnType = typeof(SortableGridColumn), Order = 2)]
        public int TotalShowTimes { get; set; }
        [GridColumnAttribute(HeaderText = "Goal reached", GridColumnType = typeof(SortableGridColumn), Order = 3)]
        public int TotalHitTimes { get; set; }
    }
}