using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Sites.Models;
namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class InheritableCheckVisible : IVisibleArbiter
    {
        public bool IsVisible(object dataItem, System.Web.Mvc.ViewContext viewContext)
        {
            var inheritableData = dataItem as Kooboo.CMS.Sites.Models.IInheritable;
            var checkable = true;
            if (inheritableData != null)
            {
                checkable = inheritableData.IsLocalized(Site.Current);

            }
            return checkable;
        }
    }
}