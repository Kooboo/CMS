using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.View
{
    [Obsolete]
    public interface IFrontPageView
    {
        Page_Context PageViewContext { get; }
    }
}
