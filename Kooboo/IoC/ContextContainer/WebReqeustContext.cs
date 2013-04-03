using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.IoC
{
    class WebReqeustContext:IRequestContext
    {
        #region IWebRequestContext Members

        public System.Collections.IDictionary Items
        {
            get {
                return HttpContext.Current.Items;
            }
        }

        #endregion
    }
}
