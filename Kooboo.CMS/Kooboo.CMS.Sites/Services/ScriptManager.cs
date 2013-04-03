using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;

namespace Kooboo.CMS.Sites.Services
{
    public class ScriptManager : PathResourceManagerBase<ScriptFile, IScriptProvider>
    {
        public override ScriptFile Get(Site site, string name)
        {
            ScriptFile script = new ScriptFile(site, name);
            if (!script.Exists())
            {
                return null;
            }
            script.Body = script.Read();
            return script;
        }
        public virtual void SaveOrder(Site site, IEnumerable<string> fileOrders)
        {
            Provider.SaveOrders(site, fileOrders);
        }

        public virtual void Localize(Site site, string fileName)
        {
            var sourceFile = (new ScriptFile(site, fileName)).LastVersion();
            Provider.Localize(sourceFile, site);
        }
    }
}
