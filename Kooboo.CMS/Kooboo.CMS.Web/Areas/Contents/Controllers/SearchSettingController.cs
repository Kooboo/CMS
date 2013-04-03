using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Search;
using Kooboo.CMS.Search.Persistence;
using Kooboo.CMS.Search.Persistence.Caching;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Web.Areas.Contents.Models;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "SearchSetting")]
    public class SearchSettingController : ManagerControllerBase
    {

        ISearchSettingProvider Provider
        {
            get
            {
                return Providers.SearchSettingProvider;
            }
        }
        //
        // GET: /Contents/SearchSetting/

        public virtual ActionResult Index()
        {
            var list = Provider.All(Repository);
            list = list.Select(o => GetEntity(o));
            return View(list);
        }
        SearchSetting GetEntity(SearchSetting model)
        {
            var r = Provider.Get(new SearchSetting(Repository, model.Name));
            r.Name = model.Name;
            return r;
        }



        public virtual ActionResult Create()
        {
            return View(new SearchSetting());
        }

        [HttpPost]
        public virtual ActionResult Create(SearchSetting model)
        {
            var entry = new JsonResultEntry();
            try
            {
                model.Repository = Repository;
                Provider.Add(model);
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }


        public virtual ActionResult Edit(string Name)
        {
            var model = Provider.Get(new SearchSetting { Name = Name, Repository = Repository });
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(SearchSetting model)
        {
            var entry = new JsonResultEntry();

            try
            {
                if (ModelState.IsValid)
                {
                    model.Repository = Repository;
                    Provider.Update(model, model);
                    entry.SetSuccess();
                }
                else
                {
                    entry.AddModelState(ModelState).SetFailed();
                }

            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        [HttpPost]
        public virtual ActionResult Delete(SearchSetting[] model)
        {
            var entry = new JsonResultEntry();

            try
            {
                if (model != null)
                {
                    foreach (var m in model)
                    {
                        m.Repository = Repository;
                        Provider.Remove(m);
                    }
                    entry.SetSuccess();
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }


        public virtual ActionResult GetFields(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                return null;
            }

            var folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(new TextFolder(Repository, FolderHelper.SplitFullName(folderPath)));

            if (folder == null)
            {
                return null;
            }


            var schema = ServiceFactory.SchemaManager.Get(Repository, folder.SchemaName);

            return Json(schema.Columns);
        }

        public virtual ActionResult IsNameAvailable(string folderName)
        {
            if (!string.IsNullOrEmpty(folderName))
            {
                if (Provider.Get(new SearchSetting(Repository, folderName)) != null)
                {
                    return Json("The folder search setting already exists.", JsonRequestBehavior.AllowGet);
                }
            }


            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
