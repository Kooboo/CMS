using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources
{
    public class QueueTypeDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            yield return new SelectListItem() { Text = "Local", Value = "4" };
            yield return new SelectListItem() { Text = "Remote", Value = "5" };
        }
    }
}