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
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.ModuleArea.Models;

namespace Kooboo.CMS.ModuleArea.Controllers
{
    public class NewsController : Controller
    {
        #region HTML
        public ActionResult Index(string category)
        {
            List<News> news = new List<News>();
            if (!string.IsNullOrEmpty(category))
            {
                for (int i = 0; i < 10; i++)
                {
                    news.Add(new News() { Id = i, Title = string.Format("{0} - news {1}", category, i), Body = "news body" });
                }
            }


            return View(news);
        }
        public ActionResult Detail(int id)
        {
            return View(new News() { Id = id, Title = "News" + id.ToString(), Body = "news body" });
        }
        public ActionResult Categories()
        {
            return View();
        }
        public ActionResult About()
        {
            return View("about");
        }
        public ActionResult LastestNews()
        {
            List<News> news = new List<News>();
            for (int i = 0; i < 5; i++)
            {
                news.Add(new News() { Id = i, Title = string.Format("lastest news {0}", i), Body = "news body" });
            }
            return View(news);
        }
        #endregion

        #region Download && json
        public ActionResult Download()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.WriteByte(0);
            return File(ms, "application/zip", "file.zip");
        }
        public ActionResult OutputJson()
        {
            return Json(new { Id = 1, Name = "Name1" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AJAX
        public ActionResult PartialRender()
        {
            return View();
        }
        public ActionResult AjaxView()
        {
            return PartialView();
        }
        #endregion
    }
}
