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
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
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
        [GridColumnAttribute(HeaderText = "Default site", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn), Order = 1)]
        [Display(Name = "Default site")]
        public string MainSite { get; set; }

        [Required]
        [UIHint("DropdownList")]
        [DataSource(typeof(VisitRuleSettingDataSource))]
        [GridColumnAttribute(HeaderText = "Rule name", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn), Order = 2)]
        [Display(Name = "Rule name")]
        public string RuleName { get; set; }

        [UIHint("ABSiteRuleItems")]
        [Display(Name = "Alternative sites")]
        public List<ABSiteRuleItem> Items { get; set; }
    }
}
