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
using System.Web;
using Kooboo.CMS.Content.Models;
using System.Collections.Specialized;

using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using System.Text;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    [Kooboo.ComponentModel.MetadataFor(typeof(ReceivingSetting))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class ReceivingSetting_Metadata
    {
        public string Repository { get; set; }

        public string Name
        {
            get;
            set;
        }

        [GridColumn(Order = 1, HeaderText = "Sending repository", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]        
        [Display(Name = "Sending repository")]
        public string SendingRepository { get; set; }

        [GridColumn(Order = 2, HeaderText = "Sending folder", GridColumnType = typeof(SortableGridColumn))]
        [UIHint("RepositoryFolderTree")]
        [Required]
        [Display(Name = "Sending folder")]
        public string SendingFolder { get; set; }

        [GridColumn(Order = 3, HeaderText = "Receiving folder", GridColumnType = typeof(SortableGridColumn))]
        [UIHint("SingleFolderTree")]
        [Required]
        [Display(Name = "Receiving folder")]
        public string ReceivingFolder { get; set; }

        [GridColumn(Order = 4, HeaderText = " content status", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        [UIHint("KeepStatus")]
        [Display(Name = "Content status")]
        public bool KeepStatus { get; set; }
    }
}