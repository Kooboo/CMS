using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc.Html
{
    public class HtmlTextbox : HtmlControlBase<HtmlTextbox>
    {
        public HtmlTextbox()
        {
            this._TagBuilder = new TagBuilder("input");
            this._TagBuilder.Attributes.Add("type", "text");
        }
        TagBuilder _TagBuilder;
        protected override TagBuilder TagBuilder
        {
            get
            {
                return this._TagBuilder;
            }
        }

        public override HtmlTextbox AddClass(string className)
        {
            this.TagBuilder.AddCssClass(className);
            return this;
        }

        public override HtmlTextbox SetAttribute(string name, string value)
        {
            this.TagBuilder.MergeAttribute(name, value, true);
            return this;
        }

        public override HtmlTextbox SetName(string name)
        {
            this.TagBuilder.MergeAttribute("name", name, true);
            return this;
        }

        public override HtmlTextbox SetId(string id)
        {
            this.TagBuilder.MergeAttribute("id", id, true);
            this.Id = id;
            return this;
        }

        public override HtmlTextbox Html(string html)
        {
            this.TagBuilder.InnerHtml = html;
            return this;
        }

        public override HtmlTextbox Text(string text)
        {
            this.TagBuilder.SetInnerText(text);
            return this;
        }

        public HtmlTextbox SetValue(string value)
        {
            this.TagBuilder.MergeAttribute("value", value, true);
            return this;
        }
    }
}
