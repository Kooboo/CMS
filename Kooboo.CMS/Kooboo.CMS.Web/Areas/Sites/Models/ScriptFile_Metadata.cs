#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Globalization;
using Kooboo.Common.Misc;
using Kooboo.Common.Web.Grid.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(ScriptFile))]
    [Grid(Checkable = true, IdProperty = "FileName", GridItemType = typeof(InheritablGridItem))]
    [GridColumn(GridColumnType = typeof(ActionGridColumn), GridItemColumnType = typeof(Localize_GridItemColumn), HeaderText = "Localize", Order = 5)]
    [GridColumn(GridColumnType = typeof(ActionGridColumn), GridItemColumnType = typeof(InheritableDialogEditGridActionItemColumn), HeaderText = "Edit", Order = 7)]
    public class ScriptFile_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [Remote("IsNameAvailable", "Script", AdditionalFields = "SiteName,FileExtension,old_Key")]
        public string Name { get; set; }

        [GridColumn(GridItemColumnType = typeof(Script_FileName_GridItemColumn))]
        public string FileName { get; set; }

        [AllowHtml]
        [UIHintAttribute("TemplateEditor")]
        public string Body { get; set; }
    }
}