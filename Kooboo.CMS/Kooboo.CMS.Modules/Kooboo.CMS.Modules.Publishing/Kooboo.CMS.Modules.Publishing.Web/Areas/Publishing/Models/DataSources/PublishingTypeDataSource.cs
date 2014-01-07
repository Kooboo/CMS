#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources
{
    public class PublishingTypeDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var enumType = typeof(PublishingType);
            var values = Enum.GetValues(enumType);
            foreach (var item in values)
            {
                yield return new SelectListItem() { Text = item.ToString(), Value = Convert.ToInt32(item).ToString() };
            }
        }
    }
}
