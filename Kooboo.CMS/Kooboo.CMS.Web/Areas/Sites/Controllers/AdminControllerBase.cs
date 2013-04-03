using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Models;
using Kooboo.Web;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public class AdminControllerBase : ControllerBase
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //var siteName = requestContext.GetRequestValue("siteName");
            //if (siteName != null)
            //{
            //    var name = siteName.ToString();
            //    if (!string.IsNullOrEmpty(name))
            //    {
            //        Site = SiteHelper.Parse(siteName).AsActual();
            //    }
            //}

        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Site == null)
            {
                filterContext.Result = RedirectToAction("index", "home");
            }
            base.OnActionExecuting(filterContext);
        }
        public virtual Site Site
        {
            get
            {
                return Site.Current;
            }
            set
            {
                Site.Current = value;
            }
        }
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Result is RedirectToRouteResult && Site != null)
            {
                ((RedirectToRouteResult)filterContext.Result).RouteValues["siteName"] = filterContext.RequestContext.GetRequestValue("siteName");
            }
            base.OnResultExecuting(filterContext);
        }
    }

    public abstract class IManagerControllerBase<T, Service> : AdminControllerBase
    where Service : IManager<T>
    {
        public virtual ActionResult Index(string search)
        {
            return View(List(search));

        }

        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Create(T model)
        {
            JsonResultEntry resultEntry = new JsonResultEntry() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    Add(model);
                }
            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);

        }


        //[CmsHandleErrorAttribute(Order = 1, RedirectToErrorPage = true)]
        public virtual ActionResult Edit(string name)
        {
            var o = Get(name);
            return View(o);
        }


        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Edit(T newModel, string old_key)
        {
            JsonResultEntry resultEntry = new JsonResultEntry() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    Update(newModel, old_key);
                    //var fromPop = ControllerContext.RequestContext.GetRequestValue("FromPop");
                    //if (string.IsNullOrWhiteSpace(fromPop))
                    //{
                    //    return RedirectToIndex(newModel.ToString());
                    //}
                }
            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);
        }

        [HttpPost]
        public virtual ActionResult Delete(T[] model)
        {
            var entry = new JsonResultEntry();
            try
            {
                foreach (var t in model)
                {
                    Remove(t);
                }
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.SetFailed().AddException(e);
            }

            return Json(entry);

        }

        protected virtual ActionResult RedirectToIndex()
        {
            return RedirectToIndex("");
        }
        protected virtual ActionResult RedirectToIndex(string newName)
        {
            var routes = this.ControllerContext.RequestContext.AllRouteValues();
            if (!string.IsNullOrEmpty(newName))
            {
                routes["name"] = newName;
            }
            return RedirectToAction("Index", routes);
        }

        public virtual Service Manager
        {
            get
            {
                return ServiceFactory.GetService<Service>();
            }
        }

        protected virtual IEnumerable<T> List(string search)
        {
            return Manager.All(Site, search);
        }

        protected virtual void Add(T model)
        {
            Manager.Add(Site, model);
        }

        protected virtual void Update(T newModel, string old_key)
        {
            Manager.Update(Site, newModel, Manager.Get(Site, old_key));
        }
        protected virtual T Get(string name)
        {
            return Manager.Get(Site, name);
        }

        protected virtual void Remove(T model)
        {
            Manager.Remove(Site, model);
        }
    }

    public abstract class PathResourceControllerBase<T, Service> : IManagerControllerBase<T, Service>
        where T : Kooboo.CMS.Sites.Models.PathResource
        where Service : IManager<T>
    {
        protected override void Update(T newModel, string old_key)
        {
            var oldModel = Manager.Get(Site, old_key);
            //newModel = Manager.Get(Site, old_key);
            //if (TryUpdateModel(newModel))  //cache...
            //{
            Manager.Update(Site, newModel, oldModel);
            //}
        }

        [HttpPost]
        public virtual ActionResult Unlocalize(T o)
        {
            var entry = new JsonResultEntry();
            try
            {
                Remove(o);
                entry.RedirectUrl = GetReturnUrl();
            }
            catch (Exception e)
            {
                entry.SetFailed().AddException(e);
            }

            return Json(entry);
        }
        protected string GetReturnUrl()
        {
            string returnUrl;
            if (!string.IsNullOrWhiteSpace(this.Request.RequestContext.GetRequestValue("ReturnUrl")))
            {
                returnUrl = this.Request.RequestContext.GetRequestValue("ReturnUrl");
            }
            else
            {
                returnUrl = Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());
            }

            return returnUrl;
        }
        

    }
}
