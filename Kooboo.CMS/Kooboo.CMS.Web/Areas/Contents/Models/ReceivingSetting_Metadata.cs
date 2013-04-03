using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Content.Models;
using System.Collections.Specialized;
using Kooboo.Web.Mvc.Grid;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using System.Text;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    [Grid(IdProperty = "Name", Checkable = true)]
    [GridAction(ActionName = "Edit", RouteValueProperties = "Name", Order = 1, Class = "o-icon edit dialog-link")]
    public class ReceivingSetting_Metadata
    {
        public string Repository { get; set; }

        public string Name
        {
            get;
            set;
        }

        [GridColumn(Order = 1)]
        [UIHint("DropDownList")]
        [DataSource(typeof(RepositoryDataSource))]
        [Display(Name = "Sending repository")]
        public string SendingRepository { get; set; }

        [GridColumn(Order = 2)]
        [UIHint("DropDownListTree")]
        [Display(Name = "Sending folder")]
        //[DataSource(typeof(FolderTreeDataSource))]
        public string SendingFolder { get; set; }


        [UIHint("SingleFolderTree")]
        [DataSource(typeof(FolderTreeDataSource))]
        [GridColumn(Order = 5)]
        [Display(Name = "Receiving folder")]
        public string ReceivingFolder { get; set; }

        [GridColumn(Order = 6, ItemRenderType = typeof(BooleanColumnRender))]
        [UIHint("KeepStatus")]
        [Display(Name = "Keep content status")]
        public bool KeepStatus { get; set; }
    }    
}