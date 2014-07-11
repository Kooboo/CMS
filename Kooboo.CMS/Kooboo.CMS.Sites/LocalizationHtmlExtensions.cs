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
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Web;
using Kooboo.Common.ObjectContainer;
namespace Kooboo.CMS.Sites
{
    public static class LocalizationHtmlExtensions
    {
        public static IHtmlString IncludeLocalizationScripts(this HtmlHelper htmlHelper)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            string jqueryUIGlobalization = string.Format("~/Scripts/jquery-ui-i18n/jquery.ui.datepicker-{0}.js", culture);
            string jqueryGlobalization = string.Format("~/Scripts/jquery-globalize/cultures/globalize.culture.{0}.js", culture);
            return new AggregateHtmlString(new IHtmlString[]{ htmlHelper.Script(htmlHelper.ResolveUrl(jqueryGlobalization)),
                htmlHelper.Script(htmlHelper.ResolveUrl(jqueryUIGlobalization))});
        }
    }
}