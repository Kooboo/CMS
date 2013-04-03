using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public class BinaryExpression : WhereFieldExpression
    {
        public BinaryExpression(IExpression expression, string fieldName, object value)
            : base(expression, fieldName)
        {
            this.Value = value;
        }
        public object Value { get; private set; }
    }
}
