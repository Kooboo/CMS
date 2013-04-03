using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace Kooboo.CMS.Sites.View
{
    public class PageDesignHtml : IHtmlString
    {
        public PageDesignHtml()
        {
            this.TagName = "var";
            this.Attribute = new NameValueCollection();
            this.Parameter = new NameValueCollection();
            this.Children = new List<IHtmlString>();
        }

        public virtual string TagName
        {
            get;
            set;
        }

        public virtual string ClassName
        {
            get;
            set;
        }

        public NameValueCollection Parameter
        {
            get;
            set;
        }

        public NameValueCollection Attribute
        {
            get;
            set;
        }

        public List<IHtmlString> Children
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.ToHtmlString();
        }

        #region IHtmlString Members

        private static void Attr(StringBuilder builder, string name, object value)
        {
            builder.AppendFormat(" {0}=\"{1}\"", name, value);
        }

        public string ToHtmlString()
        {
            // start
            var builder = new StringBuilder();

            // start tag
            builder.AppendFormat("<{0}", this.TagName);

            // write css class
            if (!string.IsNullOrEmpty(this.ClassName))
            {
                Attr(builder, "class", this.ClassName);
            }

            // write attributes
            for (var i = 0; i < this.Attribute.Count; i++)
            {
                var name = this.Attribute.Keys[i];
                Attr(builder, name, this.Attribute[name]);
            }

            // write paramters
            var parameterNames = new List<string>();
            for (var i = 0; i < this.Parameter.Count; i++)
            {
                var name = this.Parameter.Keys[i];
                Attr(builder, name, this.Parameter[name]);
                parameterNames.Add(name);
            }

            // write parameter names
            if (this.Parameter.Count > 0)
            {
                Attr(builder, "ParameterNames", string.Join(",", parameterNames.ToArray()));
            }

            builder.AppendLine(">");

            // write children
            foreach (var c in this.Children)
            {
                builder.AppendLine(c.ToHtmlString());
            }

            // end tag
            builder.AppendFormat("</{0}>", this.TagName);

            // ret
            return builder.ToString();
        }

        #endregion
    }
}
