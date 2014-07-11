#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.Web.Grid;
using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class InheritablGridItem : GridItem
    {
        public InheritablGridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {
        }
        public override IHtmlString RenderItemContainerAtts()
        {
            var @class = GetCssClass();
            if (!string.IsNullOrEmpty(@class))
            {
                return new HtmlString("class='" + @class + "'");
            }
            else
            {
                return new HtmlString("");
            }

        }
        protected virtual string GetCssClass()
        {
            var inheritableData = DataItem as Kooboo.CMS.Sites.Models.IInheritable;
            var @class = "";
            if (inheritableData != null)
            {
                var localized = inheritableData.IsLocalized(Site.Current);
                if (localized)
                {
                    @class = "localized";
                }
                else
                {
                    @class = "unlocalized";
                }
            }
            return @class;
        }
    }
}