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
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
namespace Kooboo.Web.Mvc.Grid2
{
    public static class GridExtensions
    {
        public static string GridTemplate = "_Grid2";
        public static IHtmlString GridFor2(this HtmlHelper html, Type modelType, IEnumerable dataSource)
        {
            return GridFor2(html, modelType, dataSource, GridTemplate);
        }
        public static IHtmlString GridFor2(this HtmlHelper html, Type modelType, IEnumerable dataSource, string templateName)
        {
            return new HtmlString(html.Partial(templateName, GridModel.CreateGridModel(modelType, dataSource, html.ViewContext)).ToString());
        }
        public static IHtmlString GridForModel2<T>(this HtmlHelper<IEnumerable<T>> html)
        {
            return html.GridFor2(typeof(T), html.ViewData.Model);
        }
        public static IHtmlString GridForModel2<T>(this HtmlHelper<IEnumerable<T>> html, string templateName)
        {
            return html.GridFor2(typeof(T), html.ViewData.Model, templateName);
        }
        public static IHtmlString GridForModel2<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, IEnumerable<TValue>>> expression)
        {
            var collectionModel = expression.Compile()(html.ViewData.Model);
            return html.GridFor2(typeof(TValue), collectionModel);
        }
    }
}
