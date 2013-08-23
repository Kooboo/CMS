
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.ViewModels.DataSources
{
    public class CategoriesDataSource : Kooboo.Web.Mvc.ISelectListDataSource
    {
        IProvider<Category> _provider;
        public CategoriesDataSource(IProvider<Category> provider)
        {
            _provider = provider;
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var siteName = Site.Current == null ? "" : Site.Current.FullName;
            var query = _provider.CreateQuery();
            if (!string.IsNullOrEmpty(siteName))
            {
                query = query.Where(it => it.SiteName == siteName);
            }
            return query.ToArray().Select(it => new System.Web.Mvc.SelectListItem()
            {
                Text = it.Title,
                Value = it.Id.ToString()
            });
        }
    }
}