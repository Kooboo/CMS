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
using System.Web.Mvc.Html;

namespace Kooboo.Web.Mvc.Html
{
    public static class RadioButtonListExtensions
    {
        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<string> items)
        {
            var selectList = new SelectList(items);
            return helper.RadioButtonList(name, selectList);
        }

        public static IHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("radio-list");

            foreach (var item in items)
            {
                var liTag = new TagBuilder("li");
                var rbValue = item.Value ?? item.Text;
                var rbId = name + "_" + rbValue;
                var radioTag = helper.RadioButton(name, rbValue, item.Selected, new { id = rbId });

                var labelTag = new TagBuilder("label");
                labelTag.MergeAttribute("for", rbId);
                labelTag.MergeAttribute("class", "inline");
                labelTag.InnerHtml = item.Text ?? item.Value;

                liTag.InnerHtml = radioTag.ToString() + labelTag.ToString();

                ulTag.InnerHtml += liTag.ToString();
            }

            return new HtmlString(ulTag.ToString());
        }
    }
}
