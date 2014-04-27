#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(UrlRedirect))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class UrlRedirect_Metadata
    {
        public UrlRedirect_Metadata()
        {
            UrlRedirect map = new UrlRedirect();
        }

        [GridColumn(Order = 1,HeaderText="Input URL/Pattern", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        [Description("An ordinary string OR regular expression to match the request URL, for example: oldpage_(\\w+).")]
        [Required(ErrorMessage = "Required")]
        [Remote("IsInputUrlAvailable", "UrlRedirect", AdditionalFields = "SiteName,old_Key")]
        [Display(Name = "Input URL/Pattern")]
        public string InputUrl { get; set; }

        [GridColumn(Order = 2,HeaderText="Output URL/Pattern", GridColumnType = typeof(SortableGridColumn))]
        [Required(ErrorMessage = "Required")]
        [Description("An ordinary string OR regular expression to replace the matched request URL, for example: newpage_$1.")]
        [Display(Name = "Output URL/Pattern")]
        public string OutputUrl { get; set; }

        [GridColumn(Order = 3, GridColumnType = typeof(SortableGridColumn), HeaderText = "Regex", GridItemColumnType = typeof(BooleanGridItemColumn))]
        [Description("Use either normal text matching or regular expression")]
        [DisplayName("Regex")]
        [UIHint("RadioButtonList")]
        [DataSource(typeof(Kooboo.CMS.Web.Areas.Sites.Models.DataSources.UrlRedirectTypesDataSource))]
        public bool Regex { get; set; }

        [GridColumn(HeaderText="Redirect type", Order = 4, GridColumnType = typeof(SortableGridColumn))]
        [UIHint("DropDownList")]
        [EnumDataType(typeof(RedirectType))]
        [Display(Name = "Redirect type")]
        public RedirectType RedirectType { get; set; }

        [GridColumn(HeaderText = "Creation date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn), Order = 5)]
        public DateTime? UtcCreationDate { get; set; }

        [GridColumn(HeaderText = "Lastest modification date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn), Order = 6)]
        public DateTime? UtcLastestModificationDate { get; set; }

        [GridColumn(HeaderText = "Editor", GridColumnType = typeof(SortableGridColumn), Order = 7)]
        public string LastestEditor { get; set; }

    }
}