using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css.Meta
{
    public class EnumType : PropertyValueType
    {
        private string _validItems;
        private string _defaultValue;

        public EnumType(string items)
            : this(items, items.Split('|')[0].Trim())
        {
        }

        public EnumType(string items, string defaultValue)
        {
            _defaultValue = defaultValue;
            _validItems = " " + items.Trim() + " ";
        }

        public override string DefaultValue
        {
            get { return _defaultValue; }
        }

        public override bool IsValid(string value)
        {
            return _validItems.Contains(" " + value.ToLower() + " ");
        }
    }
}
