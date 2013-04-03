using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Extension;
using System.Web.Mvc;
using Kooboo.Dynamic;

namespace Kooboo.CMS.Sites.View
{
    public class PagePositionContext
    {
        public PagePositionContext(Models.View view, IDictionary<string, object> parameters, ViewDataDictionary viewData)
        {
            this.View = view.AsActual();
            this.Parameters = new DynamicDictionary(View.CombineDefaultParameters(parameters));
            this.ViewData = viewData;
        }

        public Models.View View { get; private set; }

        public ViewDataDictionary ViewData { get; private set; }

        public IDictionary<string, object> Parameters
        {
            get;
            private set;
        }

    }
}
