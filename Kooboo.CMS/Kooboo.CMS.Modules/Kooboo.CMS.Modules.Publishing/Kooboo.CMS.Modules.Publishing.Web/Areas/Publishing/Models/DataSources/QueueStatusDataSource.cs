using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources
{
    public class QueueStatusDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var enumType = typeof(QueueStatus);
            var values = Enum.GetValues(enumType);
            foreach (var item in values)
            {
                yield return new SelectListItem() { Text = item.ToString(), Value = Convert.ToInt32(item).ToString() };
            }
        }
    }
}