#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Sites.TemplateEngines.NVelocity.MvcViewEngine
{
    public class UrlExtensionDuck : ExtensionDuck
    {
        public static readonly Type[] URL_EXTENSION_TYPES =
           new Type[]
				{				
                    typeof(Kooboo.CMS.Sites.View.UrlExtensions)
				};

        public UrlExtensionDuck(RequestContext requestContext)
            : this(new UrlHelper(requestContext))
        {
        }

        public UrlExtensionDuck(UrlHelper urlHelper)
            : this(urlHelper, URL_EXTENSION_TYPES)
        {
        }

        public UrlExtensionDuck(UrlHelper urlHelper, params Type[] extentionTypes)
            : base(urlHelper, extentionTypes)
        {
        }
    }
}
