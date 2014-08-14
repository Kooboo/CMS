using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Areas.Sites.Buttons.Home.SiteMap
{
    public class Edit_SiteMapNode : SiteMapNodeButtonBase
    {
        public override string DisplayText
        {
            get { return "Edit"; }
        }

        public override string Name
        {
            get { return "Edit_SiteMapNode"; }
        }

        public override int Order
        {
            get { return 2; }
        }
    }
}