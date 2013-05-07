#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
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
