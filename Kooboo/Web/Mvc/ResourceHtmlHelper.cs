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
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Compilation;

namespace Kooboo.Web.Mvc
{
    public static class ResourceHtmlHelper
    {
        public static string Resource(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            string virtualPath = GetVirtualPath(htmlhelper);
            return GetResourceString(htmlhelper.ViewContext.HttpContext, expression, virtualPath, args);
        }

        public static string Resource(this Controller controller, string expression, params object[] args)
        {
            return GetResourceString(controller.HttpContext, expression, "~/", args);
        }

        private static string GetResourceString(HttpContextBase httpContext, string expression, string virtualPath, object[] args)
        {
            ExpressionBuilderContext context = new ExpressionBuilderContext(virtualPath);
            ResourceExpressionBuilder builder = new ResourceExpressionBuilder();
            ResourceExpressionFields fields = (ResourceExpressionFields)builder.ParseExpression(expression, typeof(string), context);

            if (!string.IsNullOrEmpty(fields.ClassKey))
                return string.Format((string)httpContext.GetGlobalResourceObject(fields.ClassKey, fields.ResourceKey, CultureInfo.CurrentUICulture), args);

            return string.Format((string)httpContext.GetLocalResourceObject(virtualPath, fields.ResourceKey, CultureInfo.CurrentUICulture), args);
        }

        private static string GetVirtualPath(HtmlHelper htmlhelper)
        {
            // Asp.net auto generate .aspx/.ascx to a class name as "View/Home/Index.aspx" -> "view_home_index_aspx"
            // So we recover the .aspx/.ascx virtual path by the class name
            string name = htmlhelper.ViewDataContainer.GetType().Name;
            int lastUnderlineIndex = name.LastIndexOf('_');

            return "~/" + name.Substring(0, lastUnderlineIndex).Replace('_', '/') + "." + name.Substring(lastUnderlineIndex + 1);
        }

        //private static string GetVirtualPath(HtmlHelper htmlhelper)
        //{
        //    WebFormView view = htmlhelper.ViewContext.View as WebFormView;

        //    if (view != null)
        //        return view.ViewPath;

        //    return null;
        //}
    }
}
