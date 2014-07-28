#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Extension;
namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    public static class PageExtensions
    {
        public static IEnumerable<Kooboo.CMS.Sites.Models.View> Views(this Page page, Site site)
        {
            var views = new List<Kooboo.CMS.Sites.Models.View>();
            if (page != null && page.PagePositions != null)
            {
                var viewPositions = page.PagePositions.OfType<ViewPosition>();
                foreach (var viewPosition in viewPositions)
                {
                    var view = new Kooboo.CMS.Sites.Models.View(site, viewPosition.ViewName).LastVersion().AsActual();
                    if (view != null)
                    {
                        views.Add(view);
                    }
                }
            }

            return views;
        }
    }
    public class FormSubmitionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                var formName = filterContext.HttpContext.Request.Form["__FormName__"];
                if (!string.IsNullOrEmpty(formName))
                {
                    var site = Page_Context.Current.PageRequestContext.Site;
                    var page = Page_Context.Current.PageRequestContext.Page;
                    var views = page.Views(site);
                    var formSetting = views.SelectMany(it => it.FormSettings ?? new FormSetting[0]).Where(it => it.Name.EqualsOrNullEmpty(formName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    if (formSetting != null)
                    {
                        var submissionSetting = new SubmissionSetting()
                        {
                            Name = "",
                            PluginType = formSetting.PluginType,
                            Settings = formSetting.Settings,
                            Site = site
                        };

                        var pluginType = Type.GetType(submissionSetting.PluginType);
                        var submissionPlugin = (ISubmissionPlugin)TypeActivator.CreateInstance(pluginType);
                        var actionResult = submissionPlugin.Submit(site, filterContext.Controller.ControllerContext, submissionSetting);

                        if (!string.IsNullOrEmpty(formSetting.RedirectTo))
                        {
                            var url = Page_Context.Current.FrontUrl.PageUrl(formSetting.RedirectTo);
                            filterContext.Result = new RedirectResult(url.ToString());
                        }
                        else if (actionResult != null)
                        {
                            filterContext.Result = actionResult;
                        }
                    }

                }
            }
        }
    }
}
