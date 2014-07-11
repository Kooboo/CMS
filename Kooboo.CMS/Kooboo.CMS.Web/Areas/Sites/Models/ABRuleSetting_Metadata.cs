using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Misc;
using Kooboo.Common.Web.Grid.Design;
using Kooboo.Common.Web.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(ABRuleSetting))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class ABRuleSetting_Metadata
    {
        [Remote("IsNameAvailable", "ABRuleSetting", AdditionalFields = "SiteName,old_Key")]
        [DisplayName("Name")]
        [Required(ErrorMessage = "Required")]
        [Description("The meaningful name of your rule collections")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [GridColumnAttribute(HeaderText = "Name", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn), Order = 1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownList")]
        [DisplayName("Rule type")]
        [DataSource(typeof(RouteTypesDatasource))]
        [Description("The random factor of your A/B test, can be based on IP, User-Agent or percentage")]
        public string RuleType { get; set; }

        public List<IVisitRule> RuleItems
        {
            get;
            set;
        }
    }
}