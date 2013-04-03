using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence
{
    public interface IImportProvider<T>
    {
        void Export(Repository repository, IEnumerable<T> models, System.IO.Stream outputStream);
        void Import(Repository repository, string destDir, System.IO.Stream zipStream, bool @override);
    }
}
