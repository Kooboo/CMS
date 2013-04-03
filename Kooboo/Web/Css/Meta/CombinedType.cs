using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css.Meta
{
    public class CombinedType : PropertyValueType
    {
        private PropertyValueType[] _subTypes;

        public CombinedType(params PropertyValueType[] subTypes)
        {
            _subTypes = subTypes;
        }

        public override string DefaultValue
        {
            get { return _subTypes[0].DefaultValue; }
        }

        public override bool IsValid(string value)
        {
            return _subTypes.Any(o => o.IsValid(value));
        }
    }
}
