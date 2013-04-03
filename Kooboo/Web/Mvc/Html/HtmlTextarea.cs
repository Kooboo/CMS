using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc.Html
{
    public class HtmlTextarea : HtmlControlBase<HtmlTextarea>
    {
        public HtmlTextarea()
        {
            this._TagBuilder = new TagBuilder("textarea");
        }
        TagBuilder _TagBuilder;
        protected override TagBuilder TagBuilder
        {
            get
            {
                return this._TagBuilder;
            }
        }

        public override HtmlTextarea AddClass(string className)
        {
            this.TagBuilder.AddCssClass(className);
            return this;
        }

        public override HtmlTextarea SetAttribute(string name, string value)
        {
            this.TagBuilder.MergeAttribute(name, value, true);
            return this;
        }

        public override HtmlTextarea SetName(string name)
        {
            this.TagBuilder.MergeAttribute("name", name, true);
            return this;
        }

        public override HtmlTextarea SetId(string id)
        {
            this.TagBuilder.MergeAttribute("id", id, true);
            this.Id = id;
            return this;
        }

        public override HtmlTextarea Html(string html)
        {
            this.TagBuilder.InnerHtml = html;
            return this;
        }

        public override HtmlTextarea Text(string text)
        {
            this.TagBuilder.SetInnerText(text);
            return this;
        }

        public HtmlTextarea SetValue(string value)
        {
            this.TagBuilder.MergeAttribute("value", value, true);
            return this;
        }

        public HtmlTextarea SetCols(int len)
        {
            this.TagBuilder.MergeAttribute("cols", len.ToString(), true);
            return this;
        }

        public HtmlTextarea SetRows(int len)
        {
            this.TagBuilder.MergeAttribute("rows", len.ToString(), true);
            return this;
        }
    }
}
