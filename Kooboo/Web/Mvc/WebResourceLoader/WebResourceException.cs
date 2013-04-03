using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Mvc.WebResourceLoader
{
    public class WebResourceException : Exception
    {
        public WebResourceException(string msg)
            : base(msg)
        {
        }        
    }
}
