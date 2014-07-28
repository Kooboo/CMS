using Kooboo.Common.ObjectContainer.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web
{
    [Dependency(typeof(Kooboo.Common.Web.Menu.IMenuInjection), Key = "TestMenuInjection")]
    public class TestMenuInjection : Kooboo.Common.Web.Menu.IMenuInjection
    {
        public void Inject(Kooboo.Common.Web.Menu.Menu menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (menu.Name == "Sites")
            {
                menu.Items.Add(new Kooboo.Common.Web.Menu.MenuItem()
                {
                    Text = "Test",
                    Name = "Test"
                });
            }
        }
    }
}