#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;


namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class CopyPageDataSource : PagesDataSource
    {
        protected override void CreateSelectItemTreeNode(RequestContext requestContext, Page page, List<System.Web.Mvc.SelectListItem> list)
        {
            string uuid = requestContext.GetRequestValue("UUID");
            if (uuid.ToLower() != page.UUID.ToLower())
            {
                base.CreateSelectItemTreeNode(requestContext,page, list);
            }
        }
    }
}