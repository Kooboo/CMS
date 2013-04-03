using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Services
{
    public class SchemaTemplateManager : ItemTemplateManager
    {
        protected override string TemplatePath
        {
            get
            {
                return Kooboo.Web.Mvc.AreaHelpers.CombineAreaFilePhysicalPath("Contents", "Templates", "Schema");
            }
        }
    }
}
