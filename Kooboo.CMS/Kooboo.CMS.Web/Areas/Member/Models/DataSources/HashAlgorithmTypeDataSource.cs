#region License
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
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Member.Models.DataSources
{
    public class HashAlgorithmTypeDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            yield return new SelectListItem() { Text = "SHA1", Value = "SHA1" };
            yield return new SelectListItem() { Text = "MD5", Value = "MD5" };
            yield return new SelectListItem() { Text = "None", Value = "None" };
        }
    }
}
