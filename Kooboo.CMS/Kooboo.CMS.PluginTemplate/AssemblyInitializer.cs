using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.PluginTemplate.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.PluginTemplate
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            Kooboo.CMS.Content.EventBus.EventBus.Subscribers.Add(new CustomContentEventSubscriber());

            //Kooboo.CMS.Sites.ControllerTypeCache.RegisterController("Kooboo.CMS.Sites.Controllers.Front.PageController", typeof(MyPageController));
        }
    }
}
