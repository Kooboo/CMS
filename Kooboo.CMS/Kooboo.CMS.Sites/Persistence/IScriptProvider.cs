using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IScriptProvider : IProvider<ScriptFile>
    {
        void Localize(ScriptFile file, Site targetSite);
        
        void SaveOrders(Site site, IEnumerable<string> filesOrder);

        void Export(IEnumerable<ScriptFile> sources, System.IO.Stream outputStream);

        void Import(Site site, System.IO.Stream zipStream, bool @override);
    }
}
