using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc.Html
{
    public class HtmlHidden:HtmlControlBase<HtmlHidden>
    {
        public HtmlHidden()
        {
            this._TagBuilder = new TagBuilder("input");
            this._TagBuilder.Attributes.Add("type", "hidden");
        }
        TagBuilder _TagBuilder;
        protected override TagBuilder TagBuilder
        {
            get
            {
                return this._TagBuilder;
            }
        }

        public override HtmlHidden AddClass(string className)
        {
            this.TagBuilder.AddCssClass(className);
            return this;
        }

        public override HtmlHidden SetAttribute(string name, string value)
        {
            this.TagBuilder.MergeAttribute(name, value, true);
            return this;
        }

        public override HtmlHidden SetName(string name)
        {
            this.TagBuilder.MergeAttribute("name", name, true);
            return this;
        }

        public override HtmlHidden SetId(string id)
        {
            this.TagBuilder.MergeAttribute("id", id, true);
            this.Id = id;
            return this;
        }

        public override HtmlHidden Html(string html)
        {
            this.TagBuilder.InnerHtml = html;
            return this;
        }

        public override HtmlHidden Text(string text)
        {
            this.TagBuilder.SetInnerText(text);
            return this;
        }

        public HtmlHidden SetValue(string value)
        {
            this.TagBuilder.MergeAttribute("value", value, true);
            return this;
        }
    }
}
