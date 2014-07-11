#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo;
using Kooboo.CMS.Account.Services;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.Common.ComponentModel;

using Kooboo.Common.Web.Grid;
using Kooboo.Common.Web.Grid.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kooboo.Common.Web.Metadata;
namespace Kooboo.CMS.Web.Areas.Sites.Models
{

    [MetadataFor(typeof(CustomError))]
    [Grid(Checkable = true, IdProperty = "StatusCode")]
    public class CustomError_Metadata
    {
        [DisplayName("Status code")]
        [GridColumnAttribute(HeaderText = "Status code", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn), Order = 1)]
        [EnumDataType(typeof(HttpErrorStatusCode))]
        [UIHintAttribute("DropDownList")]
        [Remote("IsStatusCodeAvailable", "CustomError", AdditionalFields = "SiteName,old_Key")]
        public HttpErrorStatusCode StatusCode { get; set; }

        [GridColumnAttribute(HeaderText = "Redirect URL", GridColumnType = typeof(SortableGridColumn), Order = 2)]
        [Required(ErrorMessage = "Required")]
        [DisplayName("Redirect URL")]
        [UIHint("AutoComplete")]
        [DataSource(typeof(AutoCompletePageListDataSouce))]
        public string RedirectUrl { get; set; }

        [GridColumn(HeaderText = "Redirect type", GridColumnType = typeof(SortableGridColumn), Order = 3)]
        [UIHint("DropDownList")]
        [EnumDataType(typeof(RedirectType))]
        [Display(Name = "Redirect type")]
        public RedirectType RedirectType { get; set; }

        [GridColumn(HeaderText = "Show error path", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn), Order = 4)]
        [Display(Name = "Show error path")]
        public bool ShowErrorPath { get; set; }
    }
}