using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public static class PageExtensions
    {
        public static Layout GetLayout(this Page page)
        {
            return (new Layout(page.Site, page.AsActual().Layout)).LastVersion();
        }
    }
}
