using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class DataSourceSetting_DataSource : ISelectListDataSource
    {
        DataSourceSettingManager _manager;
        public DataSourceSetting_DataSource(DataSourceSettingManager manager)
        {
            _manager = manager;
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var site = Site.Current;
            var dataSources = _manager.All(site, null);
            return dataSources.Select(it => new SelectListItem { Text = it.UUID, Value = it.UUID });
        }
    }
}