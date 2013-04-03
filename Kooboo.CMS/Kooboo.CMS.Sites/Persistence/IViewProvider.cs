using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IViewProvider : IProvider<Kooboo.CMS.Sites.Models.View>, ILocalizableProvider<Kooboo.CMS.Sites.Models.View>
    {
        Models.View Copy(Site site, string sourceName, string destName);

        void Export(IEnumerable<Kooboo.CMS.Sites.Models.View> sources, System.IO.Stream outputStream);

        void Import(Site site, System.IO.Stream zipStream, bool @override);
    }
}
