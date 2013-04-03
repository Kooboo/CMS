using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css.Meta
{
    public class UrlType : PropertyValueType
    {
        public const string None = "none";

        public override string DefaultValue
        {
            get { return None; }
        }

        public override bool IsValid(string value)
        {
            value = value.ToLower();
            return IsUrl(value) || IsNone(value);
        }

        public override string Standardlize(string value)
        {
            return value.Replace("\"", String.Empty).Replace("'", String.Empty);
        }

        private bool IsUrl(string value)
        {
            return value.StartsWith("url(", StringComparison.OrdinalIgnoreCase)
                && value.EndsWith(")");
        }

        private bool IsNone(string value)
        {
            return value == None;
        }
    }
}
