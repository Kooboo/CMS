using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Versioning
{
    public interface IVersionable
    {
        string UserName { get; set; }
    }
}
