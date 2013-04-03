using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
namespace Kooboo.Web.Mvc.Grid
{
    public static class GridExtensions
    {
        public static IHtmlString GridFor(this HtmlHelper html, Type modelType, IEnumerable dataSource)
        {
            return GridFor(html, modelType, dataSource, "grid");
        }
        public static IHtmlString GridFor(this HtmlHelper html, Type modelType, IEnumerable dataSource, string templateName)
        {
            return new HtmlString(html.Partial(templateName, new GridModel(modelType, dataSource, html.ViewContext)).ToString());
        }
        public static IHtmlString GridForModel<T>(this HtmlHelper<IEnumerable<T>> html)
        {
            return html.GridFor(typeof(T), html.ViewData.Model);
        }
        public static IHtmlString GridForModel<T>(this HtmlHelper<IEnumerable<T>> html, string templateName)
        {
            return html.GridFor(typeof(T), html.ViewData.Model, templateName);
        }
        public static IHtmlString GridForModel<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, IEnumerable<TValue>>> expression)
        {
            var collectionModel = expression.Compile()(html.ViewData.Model);
            return html.GridFor(typeof(TValue), collectionModel);
        }
    }
}
