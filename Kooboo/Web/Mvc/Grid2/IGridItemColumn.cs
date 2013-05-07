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
using System.Text;
using System.Web;
using Kooboo.Web.Mvc.Grid2.Design;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc.Grid2
{
    public interface IGridItemColumn
    {
        IGridColumn GridColumn { get; }

        /// <summary>
        /// 行数据对象
        /// </summary>
        object DataItem { get; }

        /// <summary>
        /// 属性值
        /// </summary>
        object PropertyValue { get; }

        /// <summary>
        /// 输出td的HTML属性
        /// </summary>
        /// <returns></returns>
        IHtmlString RenderItemColumnContainerAtts(ViewContext viewContext);
        /// <summary>
        /// 输出td的HTML文本
        /// </summary>
        /// <returns></returns>
        IHtmlString RenderItemColumn(ViewContext viewContext);
    }


    public class GridItemColumn : IGridItemColumn
    {
        public GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
        {
            this.GridColumn = gridColumn;
            this.DataItem = dataItem;
            this.PropertyValue = propertyValue;
        }

        public virtual IGridColumn GridColumn
        {
            get;
            private set;
        }

        public virtual object DataItem
        {
            get;
            private set;
        }

        public virtual object PropertyValue
        {
            get;
            private set;
        }

        public virtual IHtmlString RenderItemColumnContainerAtts(ViewContext viewContext)
        {
            return new HtmlString("");
        }

        public virtual IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            return new HtmlString((PropertyValue == null || PropertyValue.ToString() == "") ? "-" : System.Web.HttpUtility.HtmlEncode(PropertyValue.ToString()));
        }
    }
}
