using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class RepositoryTemplateDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            return Kooboo.CMS.Content.Services.ServiceFactory.RepositoryTemplateManager.All().Select(it => new SelectListItem()
            {
                Text = it.TemplateName,
                Value = it.TemplateName
            }).EmptyItem("   ");
        }
    }
    public class CreateRepositoryModel
    {
        [Required(ErrorMessage = "Required")]
        [Remote("IsNameAvailable", "Repository")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Name { get; set; }

        [UIHint("DropDownList")]
        [DataSource(typeof(RepositoryTemplateDataSource))]
        public string Template { get; set; }
    }
}