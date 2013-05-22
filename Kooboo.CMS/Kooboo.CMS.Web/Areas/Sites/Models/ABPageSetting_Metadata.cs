#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
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
    [MetadataFor(typeof(ABPageSetting))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class ABPageSetting_Metadata
    {
        [RemoteEx("IsNameAvailable", "ABPageSetting", RouteFields = "SiteName")]
        [Required]
        [UIHint("DropdownList")]
        [DataSource(typeof(PagesDataSource))]
        [GridColumnAttribute(HeaderText = "Entry page", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn), Order = 1)]
        [Display(Name = "Entry page")]
        [Description("The entry and default page that this rule will apply to.")]
        public string MainPage { get; set; }

        [Required]
        [UIHint("DropdownList")]
        [DataSource(typeof(ABRuleSettingDataSource))]
        [GridColumnAttribute(HeaderText = "Rule name", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn), Order = 2)]
        [Display(Name = "Rule name")]
        public string RuleName { get; set; }

        [UIHint("ABPageRuleItems")]
        [Display(Name = "Alternative pages")]
        [Description("The alternative pages to render based on defined A/B rules.")]
        public List<ABPageRuleItem> Items { get; set; }

        [UIHint("DropdownList")]
        [GridColumnAttribute(HeaderText = "Goal page", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn), Order = 1)]
        [DisplayName("Goal page")]
        [DataSource(typeof(PagesDataSource))]
        [Description("The destnation page to measure the result of individual pages that come from A/B test.")]
        public string ABTestGoalPage { get; set; }
    }
}
