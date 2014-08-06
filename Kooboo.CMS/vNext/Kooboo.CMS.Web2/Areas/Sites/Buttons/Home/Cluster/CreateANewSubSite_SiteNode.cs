using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Areas.Sites.Buttons.Home.Cluster
{
    public class CreateANewSubSite_SiteNode : CreateANewSubSite_Top
    {
        public override string GroupName
        {
            get
            {
                return null;
            }
        }
        public override string Position
        {
            get
            {
                return Kooboo.CMS.SiteKernel.Extension.Site.SiteExtensionPoints.SiteNodeButton;
            }
        }
    }
}