﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Contents.Models.DataSources
{
    public class ControlTypesDataSource : ISelectListDataSource
    {

        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var controls = Kooboo.CMS.Form.Html.ControlHelper.ResolveAll();
            foreach (var c in controls)
            {
                yield return new Kooboo.Web.Mvc.SelectListItemEx() { Text = c.Name, Value = c.Name, HtmlAttributes = new Dictionary<string, object>() { { "data-datatype", c.DataType } } };
            }
        }
    }

}