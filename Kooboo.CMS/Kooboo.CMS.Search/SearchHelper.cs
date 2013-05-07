#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.Web.Mvc.Paging;

namespace Kooboo.CMS.Search
{
    public static class SearchHelper
    {
        static SearchHelper()
        {
            ServiceBuilder = new ServiceBuilder();
        }
        public static IServiceBuilder ServiceBuilder { get; set; }

        public static ISearchService OpenService(Repository repository)
        {
            return ServiceBuilder.OpenService(repository);
        }

        public static PagedList<Models.ResultObject> Search(this Repository repository, string key, int pageIndex, int pageSize, params string[] folders)
        {
            return OpenService(repository).Search(key, pageIndex, pageSize, folders);
        }
    }
}
