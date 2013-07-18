#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.CMS.Web.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(ABSiteSetting))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class ABSiteSetting_Metadata
    {
        [Remote("IsNameAvailable", "ABSiteSetting")]
        [Required]
        [UIHint("DropdownList")]
        [DataSource(typeof(SitesDataSource))]
        [GridColumnAttribute(HeaderText = "Entry site", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn), Order = 1)]
        [Display(Name = "Entry site")]
        [Description("The entry and default website that this rule will apply to.")]
        public string MainSite { get; set; }

        [Required]
        [UIHint("DropdownList")]
        [DataSource(typeof(ABRuleSettingDataSource))]
        [GridColumnAttribute(HeaderText = "Rule name", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn), Order = 2)]
        [Display(Name = "Rule name")]
        public string RuleName { get; set; }

        [UIHint("ABSiteRuleItems")]
        [Display(Name = "Alternative sites")]
        [Description("The alternative sites to render based on defined A/B rules.")]
        public List<ABSiteRuleItem> Items { get; set; }

        [GridColumn(HeaderText = "Redirect type", GridColumnType = typeof(SortableGridColumn), Order = 3)]
        [UIHint("DropDownList")]
        [EnumDataType(typeof(RedirectType))]
        [Display(Name = "Redirect type")]
        public RedirectType? RedirectType { get; set; }
    }
}
