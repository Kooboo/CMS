#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers
{
    public class DataSourceController : FrontControllerBase
    {
        DataSourceSettingManager _dataSourceSettingManager;
        public DataSourceController(DataSourceSettingManager dataSourceSettingManager)
        {
            _dataSourceSettingManager = dataSourceSettingManager;
        }
        public ActionResult Index(string dataSourceName)
        {
            var dataSource = _dataSourceSettingManager.Get(Site, dataSourceName);

            var dataSourceContext = new DataSourceContext(Site, null) { ValueProvider = ValueProvider };

            var data = dataSource.DataSource.Execute(dataSourceContext);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
