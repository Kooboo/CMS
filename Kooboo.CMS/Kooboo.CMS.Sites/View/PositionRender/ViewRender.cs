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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using System.Globalization;


namespace Kooboo.CMS.Sites.View.PositionRender
{
    using Kooboo.CMS.Sites.DataRule;
    using Kooboo.CMS.Common.Persistence.Non_Relational;
    using Kooboo.CMS.Sites.Caching;
    using System.Runtime.Caching;

    public class ViewRender
    {
        #region Static
        public static string GeneratedByViewComment = @"
<!--View:{0}-->
{1}
<!--/View:{0}-->";
        #endregion
        
        public virtual IHtmlString RenderView(HtmlHelper htmlHelper, Page_Context pageContext, string viewName, ViewDataDictionary viewData, object parameters, bool executeDataRule)
        {
            Kooboo.CMS.Sites.Models.View view = (new Kooboo.CMS.Sites.Models.View(pageContext.PageRequestContext.Site, viewName).LastVersion()).AsActual();

            if (view != null)
            {
                //backup the parent view context
                var parentPositionContext = pageContext.ViewDataContext;

                var parameters1 = parameters is IDictionary<string, object> ? ((IDictionary<string, object>)parameters) : new RouteValueDictionary(parameters);
                pageContext.ViewDataContext = new PagePositionContext(view, parameters1, viewData);

                if (executeDataRule)
                {
                    viewData = new ViewDataDictionary(viewData);
                    var pageRequestContext = pageContext.PageRequestContext;
                    if (view.DataRules != null)
                    {
                        var valueProvider = pageRequestContext.GetValueProvider();
                        valueProvider.Insert(0, new ViewParameterValueProvider(pageContext.ViewDataContext.Parameters));
                        var dataRuleContext = new DataRuleContext(pageRequestContext.Site, pageRequestContext.Page) { ValueProvider = valueProvider };
                        DataRuleExecutor.Execute(viewData, dataRuleContext, view.DataRules);
                    }
                }

                var html = RenderViewInternal(htmlHelper, view.TemplateFileVirutalPath, viewData, null);


                if (pageContext.EnableTrace)
                {
                    html = new HtmlString(string.Format(GeneratedByViewComment, viewName, html.ToString()));
                }

                //restore the parent view context
                pageContext.ViewDataContext = parentPositionContext;

                return html;
            }
            else
            {
                return new HtmlString("");
            }
        }

        internal IHtmlString RenderViewInternal(HtmlHelper htmlHelper, string viewPath, ViewDataDictionary viewData, object model)
        {
            if (string.IsNullOrEmpty(viewPath))
            {
                throw new ArgumentException("", "viewPath");
            }
            ViewDataDictionary dictionary = null;
            if (model == null)
            {
                if (viewData == null)
                {
                    dictionary = new ViewDataDictionary(htmlHelper.ViewData);
                }
                else
                {
                    dictionary = new ViewDataDictionary(viewData);
                }
            }
            else if (viewData == null)
            {
                dictionary = new ViewDataDictionary(model);
            }
            else
            {
                dictionary = new ViewDataDictionary(viewData)
                {
                    Model = model
                };
            }
            StringWriter writer = new StringWriter(CultureInfo.CurrentCulture);

            ViewContext viewContext = new ViewContext(htmlHelper.ViewContext, htmlHelper.ViewContext.View, dictionary, htmlHelper.ViewContext.TempData, writer);
            TemplateEngines.GetEngineByFileExtension(Path.GetExtension(viewPath)).CreateView(htmlHelper.ViewContext.Controller.ControllerContext, viewPath, null).Render(viewContext, writer);

            return new HtmlString(writer.ToString());
        }
    }
}
