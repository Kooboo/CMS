using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.Sites.TemplateEngines.NVelocity.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.Sites.TemplateEngines.NVelocity
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            ApplicationInitialization.RegisterInitializerMethod(delegate()
            {
                System.Web.Mvc.ViewEngines.Engines.Add(Kooboo.CMS.Sites.TemplateEngines.NVelocity.MvcViewEngine.NVelocityViewEngine.Default);
                Kooboo.CMS.Sites.View.TemplateEngines.RegisterEngine(new NVelocityTemplateEngine());
            }, 0);
        }
    }
}
