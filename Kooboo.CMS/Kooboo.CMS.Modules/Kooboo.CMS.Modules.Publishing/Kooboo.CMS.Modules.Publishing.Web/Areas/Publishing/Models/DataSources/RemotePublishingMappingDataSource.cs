#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources
{
    public class RemotePublishingMappingDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var provider = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IRemoteTextFolderMappingProvider>();
            var settings = provider.CreateQuery(Kooboo.CMS.Sites.Models.Site.Current.Name).Where(it => it.Enabled);
            foreach (var set in settings)
            {
                yield return new SelectListItem() { Text = set.Name, Value = set.Name };
            }
        }
    }
}
