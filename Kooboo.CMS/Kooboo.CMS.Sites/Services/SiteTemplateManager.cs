using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Services
{
    public class SiteTemplateManager : ItemTemplateManager
    {
        protected override string BasePath
        {
            get { return Kooboo.Web.Mvc.AreaHelpers.CombineAreaFilePhysicalPath("Sites", "Templates", "Site"); }
        }
    }
}
