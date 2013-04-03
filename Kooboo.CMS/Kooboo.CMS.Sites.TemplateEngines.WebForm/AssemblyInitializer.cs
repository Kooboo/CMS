using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.Sites.TemplateEngines.WebForm.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.Sites.TemplateEngines.WebForm
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            ApplicationInitialization.RegisterInitializerMethod(delegate()
            {
                Kooboo.CMS.Sites.View.TemplateEngines.RegisterEngine(new WebFormTemplateEngine());
            }, 0);
        }
    }
}
