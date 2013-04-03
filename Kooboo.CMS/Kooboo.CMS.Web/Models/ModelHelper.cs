using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using System.Web.Mvc;

using System.Web.Mvc.Html;

using Kooboo.Web.Mvc;
using System.Web.Routing;

using Kooboo.Globalization;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Web.Models
{
    public static class ModelHelper
    {
        public static T ParseViewData<T>(object data) where T : new()
        {
            return data != null ? (T)data : new T();
        }

        public static IEnumerable<T> ParseViewDataToList<T>(object data) where T : new()
        {
            return data != null ? (IEnumerable<T>)data : new List<T>();
        }
    }

    public class CommonLinkNameColumnRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            if (dataItem is IInheritable)
            {
                var inheritable = (IInheritable)dataItem;
                if (Site.Current != inheritable.Site)
                {
                    return new HtmlString(value == null ? "" : value.ToString());
                }
            }
            UrlHelper url = new UrlHelper(viewContext.RequestContext);
            return new HtmlString(string.Format(@"<a href=""{0}"">{1}</a>", url.Action("Edit", viewContext.RequestContext.AllRouteValues().Merge("Name", value)), value));

        }
    }

    public class CommonLinkPopColumnRender : IItemColumnRender
    {

        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            if (dataItem is IInheritable)
            {
                var inheritable = (IInheritable)dataItem;
                if (Site.Current != inheritable.Site)
                {
                    return new HtmlString(value == null ? "" : value.ToString());
                }
            }


            return new HtmlString(string.Format(@"<a href=""{0}"" class=""dialog-link"" title=""{1}"">{2}</a>", GetUrl(dataItem, value, viewContext), GetTitle(dataItem, value, viewContext), GetDisplayText(dataItem, value, viewContext)));
        }

        public virtual string GetDisplayText(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            return value.ToString();
        }

        public virtual string GetUrl(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            UrlHelper url = new UrlHelper(viewContext.RequestContext);
            return url.Action("Edit", viewContext.RequestContext.AllRouteValues().Merge("Name", value));
        }

        public virtual string GetTitle(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            return "Edit".Localize();
        }
    }


}