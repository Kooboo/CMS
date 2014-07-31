#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Data;
using Kooboo.Common.Web.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class PagerExtensions
    {
        #region Pager
        internal class CmsPagerBuilder : PagerBuilder
        {
            private RouteValueDictionary routeValues;
            private IPagedList _pageList = null;
            private readonly PagerOptions _pagerOptions;
            private IFrontHtmlHelper frontHtml;
            internal CmsPagerBuilder(IFrontHtmlHelper html, IPagedList pageList,
                PagerOptions pagerOptions, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
                : base(html.Html, null, null, pageList, pagerOptions, null, routeValues, htmlAttributes)
            {
                frontHtml = html;
                this.routeValues = routeValues ?? new RouteValueDictionary();


                _pageList = pageList;
                this._pagerOptions = pagerOptions;
            }
            protected override string GenerateUrl(int pageIndex)
            {
                //return null if  page index larger than total page count or page index is current page index
                if (pageIndex > _pageList.TotalPageCount || pageIndex == _pageList.CurrentPageIndex)
                    return null;

                var routeValues = this.routeValues.Merge(frontHtml.Page_Context.PageRequestContext.AllQueryString.ToRouteValues())
                    .Merge(_pagerOptions.PageIndexParameterName, pageIndex);
                return frontHtml.Page_Context.FrontUrl
                    .GeneratePageUrl(frontHtml.Page_Context.PageRequestContext.Page, routeValues).ToString();
            }
        }
        public static IHtmlString Pager(this IFrontHtmlHelper frontHtml, object model)
        {
            return Pager(frontHtml, model, null);
        }
        public static IHtmlString Pager(this IFrontHtmlHelper frontHtml, object model, PagerOptions options)
        {
            return Pager(frontHtml, model, null, options, null);
        }
        //Optional parameter does not support NVelocity
        public static IHtmlString Pager(this IFrontHtmlHelper frontHtml, object model, object routeValues, PagerOptions options, object htmlAttributes)
        {
            options = options ?? new PagerOptions();

            //if ((model is DataRulePagedList))
            //{
            //    options.PageIndexParameterName = ((DataRulePagedList)model).PageIndexParameterName;
            //}

            var pagedList = (IPagedList)model;

            var builder = new CmsPagerBuilder
             (
                 frontHtml,
                 pagedList,
                 options,
                 RouteValuesHelper.GetRouteValues(routeValues),
                 RouteValuesHelper.GetRouteValues(htmlAttributes)
             );
            return new HtmlString(builder.RenderPager().ToString());
        }
        #endregion
    }
}
