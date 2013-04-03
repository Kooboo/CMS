using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc.Html
{
    public abstract class HtmlControlBase<T>:IHtmlControl where T:IHtmlControl
    {
        protected abstract TagBuilder TagBuilder
        {
           get;
        }

        protected virtual TagRenderMode TagRenderMode
        {
            get
            {
                return TagRenderMode.Normal;
            }
        }



        #region IHtmlControl Members

        IHtmlControl IHtmlControl.AddClass(string className)
        {
            this.TagBuilder.AddCssClass(className);
            return this;
        }

        IHtmlControl IHtmlControl.SetAttribute(string name, string value)
        {
            this.TagBuilder.MergeAttribute(name, value, true);
            return this;
        }


        IHtmlControl IHtmlControl.SetName(string name)
        {
            this.TagBuilder.MergeAttribute("name", name, true);
            this.Name = name;
            return this;
        }

        IHtmlControl IHtmlControl.SetId(string id)
        {
            this.TagBuilder.MergeAttribute("id", id, true);
            this.Id = id;
            return this;
        }

        IHtmlControl IHtmlControl.Html(string html)
        {
            this.TagBuilder.InnerHtml = html;
            return this;
        }

        IHtmlControl IHtmlControl.Text(string text)
        {
            this.TagBuilder.SetInnerText(text);
            return this;
        }
        #endregion

        public IDictionary<string, string> Attributes
        {
            get {
                return this.TagBuilder.Attributes;
            }
        }


        public string Name
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public System.Web.Mvc.MvcHtmlString ToHtmlString()
        {
            return MvcHtmlString.Create(this.ToString());
        }

        public override string ToString()
        {
            return this.TagBuilder.ToString(this.TagRenderMode);
        }


        public abstract T AddClass(string className);

        public abstract T SetAttribute(string name, string value);

        public abstract T SetName(string name);

        public abstract T SetId(string id); 

        public abstract T Html(string html);

        public abstract T Text(string text);
    }
}
