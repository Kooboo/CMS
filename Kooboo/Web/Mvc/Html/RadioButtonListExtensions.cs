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
            TagBuilder divTag = new TagBuilder("div");
            divTag.AddCssClass("radio-list");

            foreach (var item in items)
            {
                var pTag = new TagBuilder("p");
                pTag.AddCssClass("clearfix");
                var rbValue = item.Value ?? item.Text;
                var rbId = name + "_" + rbValue;
                var radioTag = helper.RadioButton(name, rbValue, item.Selected, new { id = rbId });

                var labelTag = new TagBuilder("label");
                labelTag.MergeAttribute("for", rbId);
                labelTag.MergeAttribute("class", "radio-label");
                labelTag.InnerHtml = item.Text ?? item.Value;

                pTag.InnerHtml = radioTag.ToString() + labelTag.ToString();

                divTag.InnerHtml += pTag.ToString();
            }

            return new HtmlString(divTag.ToString());
        }
    }
}
