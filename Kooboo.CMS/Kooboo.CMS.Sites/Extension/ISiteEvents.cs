using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.Web;

namespace Kooboo.CMS.Sites.Extension
{
    public interface ISiteEvents
    {
        void OnSiteStart(Site site);
        void OnPreSiteRequestExecute(Site site, HttpContextBase httpContext);
        void OnPostSiteRequestExecute(Site site, HttpContextBase httpContext);
        void OnSiteRemoved(Site site);
    }
}
