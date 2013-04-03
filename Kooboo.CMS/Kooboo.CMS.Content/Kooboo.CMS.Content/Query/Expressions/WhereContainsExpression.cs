using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public class WhereContainsExpression : BinaryExpression
    {
        public WhereContainsExpression(IExpression expression, string fieldName, object value)
            : base(expression, fieldName, value)
        {

        }

    }
}
