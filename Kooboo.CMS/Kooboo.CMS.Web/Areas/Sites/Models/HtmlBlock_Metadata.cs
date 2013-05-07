#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(HtmlBlock))]
    [Grid(Checkable = true, IdProperty = "UUID", GridItemType = typeof(InheritablGridItem))]
    public class HtmlBlock_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(InheritableEditGridActionItemColumn))]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [RemoteEx("IsNameAvailable", "HtmlBlock", AdditionalFields = "SiteName,old_Key")]
        public string Name { get; set; }

        [UIHint("Tinymce")]
        public string Body { get; set; }
    }
}