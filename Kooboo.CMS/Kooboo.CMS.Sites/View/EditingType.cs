using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.View
{
    [Flags]
    public enum EditingType
    {
        Label = 1,
        Content = 2,
        Page = 4
    }
}
