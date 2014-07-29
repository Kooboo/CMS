#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web2.Models
{
    public class TimeZonesDataSource : ISelectListDataSource
    {
        ITimeZoneHelper _timeZoneHelper;
        public TimeZonesDataSource(ITimeZoneHelper timeZoneHelper)
        {
            _timeZoneHelper = timeZoneHelper;
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            return _timeZoneHelper.GetTimeZones().Select(it => new SelectListItem()
            {
                Selected = TimeZoneInfo.Local.Id == it.Id,
                Text = it.DisplayName,
                Value = it.Id
            });
        }
    }
}