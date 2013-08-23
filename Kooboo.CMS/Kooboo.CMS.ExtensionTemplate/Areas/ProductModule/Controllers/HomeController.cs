using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Kooboo.CMS.Content.Query;
namespace Kooboo.CMS.ExtensionTemplate.Areas.ProductModule.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var productFolder = new TextFolder(Repository.Current, "Product");

            return View(productFolder.CreateQuery());
        }
        public ActionResult Detail(string userKey)
        {
            var productFolder = new TextFolder(Repository.Current, "Product");
            var product = productFolder.CreateQuery().WhereEquals("UserKey", userKey).FirstOrDefault();
            return View(product);
        }
    }
}
