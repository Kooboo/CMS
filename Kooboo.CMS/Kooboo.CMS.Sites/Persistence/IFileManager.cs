using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.IO;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IFileManager
    {      
        void Unzip(string directory, Stream zipStream);
    }
}
