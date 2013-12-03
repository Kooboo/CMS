using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Extension.ModuleArea;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Controllers
{
    public class HomeController : Controller
    {
        #region .ctor
        IProvider<Category> _categoryProvider;
        IProvider<Blog> _blogProvider;
        public HomeController(IProvider<Category> categoryProvider, IProvider<Blog> blogProvider)
        {
            _categoryProvider = categoryProvider;
            _blogProvider = blogProvider;
        }
        #endregion

        #region Index
        public ActionResult Index(int id = 0)
        {
            var blogs = this.ControllerContext.GetSharedData("Blogs");

            if (blogs == null)
            {
                var blogQuery = _blogProvider.CreateQuery().Where(it => it.SiteName == Site.Current.FullName);

                if (id != 0)
                {
                    blogQuery = blogQuery.Where(it => it.Categories.Any(c => c.Id == id));
                }
                blogs = blogQuery;
            }
            return View(blogs);
        }
        #endregion

        #region Category
        public ActionResult Category(int id = 0)
        {
            var categories = _categoryProvider.CreateQuery().Where(it => it.SiteName == Site.Current.FullName).ToArray();

            if (id != 0)
            {
                var blogQuery = _blogProvider.CreateQuery().Where(it => it.SiteName == Site.Current.FullName);
                var blogs = blogQuery.Where(it => it.Categories.Any(c => c.Id == id));
                this.ControllerContext.ShareData("Blogs", blogs);
            }
            return View(categories);
        }
        #endregion
        #region Detail
        public ActionResult Detail(int id = 0)
        {
            var blog = _blogProvider.CreateQuery().Where(it => it.Id == id).FirstOrDefault();
            return View(blog);
        }
        #endregion

        #region LastestBlogs
        public ActionResult LastestBlogs()
        {
            var blogQuery = _blogProvider.CreateQuery().Where(it => it.SiteName == Site.Current.FullName);
            var lastestBlogs = blogQuery.OrderByDescending(it => it.CreateionDate).Take(5);
            return View(lastestBlogs);
        }
        #endregion
    }
}
