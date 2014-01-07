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

namespace Kooboo.Web.Mvc
{
    public static class ViewContextExtensions
    {
        public static bool IsHandledBy<TController>(this ViewContext context)
        {
            return context.Controller is TController;
        }

        private class ViewDataContainer : IViewDataContainer
        {
            public ViewDataContainer(ViewDataDictionary viewData)
            {
                ViewData = viewData;
            }


            public ViewDataDictionary ViewData
            {
                get;
                set;
            }
        }
        public static HtmlHelper HtmlHelper(this ViewContext context)
        {
            return new HtmlHelper(context, new ViewDataContainer(context.ViewData));
        }
        public static UrlHelper UrlHelper(this ViewContext context)
        {
            return new UrlHelper(context.RequestContext);
        }
    }
}
