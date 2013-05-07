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
using System.Web.Mvc;
using Kooboo.Reflection;
namespace Kooboo.Web.Mvc.Grid
{
    public class GridColumn
    {
        public GridColumn()
        {

        }

        public string PropertyName { get; set; }
        public string HeaderText { get; set; }

        public IHtmlString GetFormattedHeaderText(ViewContext viewContext)
        {
            IHtmlString formattedText = null;
            if (HeaderRender != null)
            {
                formattedText = HeaderRender.Render(HeaderText, viewContext);
            }
            else
            {
                formattedText = new HtmlString(System.Web.HttpUtility.HtmlEncode(HeaderText));
            }
            if (!string.IsNullOrEmpty(HeaderFormatString))
            {
                formattedText = new HtmlString(string.Format(HeaderFormatString, formattedText));
            }

            return formattedText;
        }

        public string HeaderFormatString { get; set; }
        public string ItemFormatString { get; set; }
        public string Class { get; set; }

        public IColumnHeaderRender HeaderRender { get; set; }
        public IItemColumnRender ItemRender { get; set; }
        public IItemColumnRender AlternatingItemRender { get; set; }

        public int Order { get; set; }

        public object GetValue(object instance)
        {
            if (string.IsNullOrEmpty(PropertyName))
            {
                return "";
            }
            return instance.Members().Properties[PropertyName];
        }
    }

}
