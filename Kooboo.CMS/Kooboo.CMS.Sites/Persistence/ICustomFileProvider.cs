using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface ICustomFileProvider : IProvider<CustomFile>
    {
        IQueryable<CustomFile> All(CustomDirectory dir);

        void Export(CustomDirectory dir, System.IO.Stream outputStream);

        void Import(Site site, CustomDirectory dir, System.IO.Stream zipStream, bool @override);
    }


}
