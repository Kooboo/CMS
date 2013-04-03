using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [Grid(Checkable = true, IdProperty = "PageName")]
    public class PagePublishingQueue_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Page name")]
        public string PageName { get; set; }

        [GridColumn(Order = 2, HeaderText = "Publish draft", ItemRenderType = typeof(BooleanColumnRender))]
        public bool PublishDraft { get; set; }

        [GridColumn(Order = 3, HeaderText = "Creation date", ItemRenderType = typeof(DateTimeColumnRender))]
        public DateTime CreationUtcDate { get; set; }

        [GridColumn(Order = 5, HeaderText = "Date to publish", ItemRenderType = typeof(DateTimeColumnRender))]
        public DateTime UtcDateToPublish { get; set; }

        [GridColumn(Order = 6, HeaderText = "Period", ItemRenderType = typeof(BooleanColumnRender))]
        public bool Period { get; set; }

        [GridColumn(Order = 7, HeaderText = "Date to offline", ItemRenderType = typeof(DateTimeColumnRender))]
        public DateTime UtcDateToOffline { get; set; }

        [GridColumn(Order = 8)]
        public string UserName { get; set; }

        [GridColumn(Order = 9)]
        public Site Site
        {
            get;
            set;
        }

    }
}