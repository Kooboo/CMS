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
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Sites.Models;
using System.Web.Mvc;
using Kooboo.Common.Globalization;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class InheritableGridActionVisibleArbiter : IVisibleArbiter
    {

        #region IGridActionVisibleArbiter Members

        public virtual bool IsVisible(object dataItem, System.Web.Mvc.ViewContext viewContext)
        {
            var inheritable = dataItem as IInheritable;
            if (inheritable != null)
            {
                return inheritable.IsLocalized(Site.Current);
            }
            else
            {
                return true;
            }

        }

        #endregion
    }
    public class LocalizationRender : IGridItemActionRender
    {
        public GridItemAction Render(object dataItem, GridItemAction itemAction, ViewContext viewContext)
        {
            var inheritable = dataItem as IInheritable;
            if (inheritable != null)
            {
                var localized = inheritable.IsLocalized(Site.Current);
                if (localized)
                {
                    //注释下面的代码，解决子页面本地化，父页面没有本地化时，不能进行还原操作。
                    var hasParent = Site.Current.Parent != null;
                    if (hasParent)
                    {
                        itemAction.ActionName = "Unlocalize";
                        itemAction.Title = "Unlocalize".Localize();
                        itemAction.Class = "o-icon unlocalize actionCommand";
                        itemAction.ConfirmMessage = "Are you sure you want to unlocalize this item?";
                        itemAction.Visible = true;
                    }
                    else
                    {
                        itemAction.Title = "Localize";
                        itemAction.Visible = false;
                    }
                }

            }

            return itemAction;

        }
    }

}