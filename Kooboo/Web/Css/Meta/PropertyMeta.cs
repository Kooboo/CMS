using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css.Meta
{
    public partial class PropertyMeta
    {
        private IEnumerable<string> _subProperties;

        public PropertyMeta(string name, PropertyValueType valueType)
        {
            Name = name;
            ValueType = valueType;
        }

        public string ShorthandName { get; internal set; }

        public string Name { get; private set; }

        public PropertyValueType ValueType { get; private set; }

        public bool IsShorthand 
        {
            get
            {
                return ValueType is ShorthandType;
            }
        }

        public IEnumerable<string> SubProperties
        {
            get
            {
                if (!IsShorthand)
                    throw new InvalidOperationException("Property can only be accessed in case of shorthand, please use IsShorthand first.");

                if (_subProperties == null)
                {
                    _subProperties = (ValueType as ShorthandType).ShorthandRule.SubProperties(this);
                }

                return _subProperties;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
