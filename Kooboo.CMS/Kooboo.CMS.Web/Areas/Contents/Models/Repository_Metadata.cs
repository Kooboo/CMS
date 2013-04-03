using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Content.Persistence;
using System.Web.Mvc;
using System.ComponentModel;
using Kooboo.CMS.Content.Services;
namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    //public class DBProvidersDataSource : ISelectListDataSource
    //{

    //    #region ISelectListDataSource Members

    //    public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Mvc.ViewContext viewContext)
    //    {
    //        foreach (var item in Providers.ProviderFactories.Keys)
    //        {
    //            yield return new SelectListItem() { Text = item, Value = item };
    //        }
    //    }

    //    #endregion
    //}
    public class Repository_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Name { get; set; }


        [DisplayName("Display name")]
        public string DisplayName { get; set; }

        [DisplayName("Enable broadcasting")]
        [Description("Enable content broadcasting and shareing between content repositories.")]
        public bool EnableBroadcasting { get; set; }

        [Description("Customize the CMS content editing page and the default front site content display")]
        [DisplayName("Custom template")]
        public bool EnableCustomTemplate { get; set; }

        //[Description("Manually input userkey <br />Userkey is an user and SEO friendly content key <br />It is mostly used to customize the page URL")]
        //[DisplayName("Manual userkey")]
        //public bool EnableManuallyUserKey { get; set; }

        //[UIHint("DropDownList")]
        //[DataSource(typeof(DBProvidersDataSource))]
        //public string DBProvider { get; set; }
        [Description("Disallowed userkey chars. Replace chars that match this regular expression into hyphens")]
        [DisplayName("Userkey escape chars")]
        public string UserKeyReplacePattern { get; set; }

        [DisplayName("Userkey hyphens")]
        public string UserKeyHyphens { get; set; }

        [DisplayName("Enable versioning")]
        [Required]
        [Description("Enable content versioning")]
        public bool? EnableVersioning { get; set; }
                
        [Required]
        [DisplayName("Enable workflow")]
        [Description("Enable workflow control on folder content adding.")]
        public bool? EnableWorkflow { get; set; }

        [DisplayName("Strict content permission")]
        [Description("User can not manage the content when the folder permission is empty.")]
        public bool StrictContentPermission { get; set; }
    }
    public class RepositoryDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            return ServiceFactory.RepositoryManager.All().Select(o => new SelectListItem { Text = o.Name, Value = o.Name });
        }
    }

}