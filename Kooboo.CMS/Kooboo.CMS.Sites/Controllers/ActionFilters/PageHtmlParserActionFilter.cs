#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.View.HtmlParsing;
using Kooboo.CMS.Sites.Web;

namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    public class PageHtmlParserActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
#if Page_Trace
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            if (Page_Context.Current.PageRequestContext != null)
            {

                var site = Page_Context.Current.PageRequestContext.Site;
                var page = Page_Context.Current.PageRequestContext.Page.AsActual();

                var outputTextWriterWrapper = filterContext.HttpContext.Response.Output as OutputTextWriterWrapper;
                if (Page_Context.Current.InlineEditing == false && outputTextWriterWrapper != null
                    && !(filterContext.Result is CachedContentResult)
                    && filterContext.Exception == null
                    && filterContext.HttpContext.Response.ContentType.ToLower().Contains("text/html"))
                {
                    var html = outputTextWriterWrapper.ToString();
                    var htmlParser = Kooboo.CMS.Common.Runtime.EngineContext.Current.TryResolve<IHtmlParser>();
                    if (htmlParser != null)
                    {
                        html = htmlParser.Parse(html);
                        outputTextWriterWrapper.Flush();
                        outputTextWriterWrapper.Write(html);
                    }
                }
            }

#if Page_Trace
            stopwatch.Stop();
            filterContext.HttpContext.Response.Write(string.Format("PageHtmlParserActionFilter.OnResultExecuted, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
#endif
        }
    }
}
