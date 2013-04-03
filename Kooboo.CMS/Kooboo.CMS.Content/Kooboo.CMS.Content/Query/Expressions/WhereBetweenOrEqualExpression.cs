using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public class WhereBetweenOrEqualExpression : WhereBetweenExpression
    {
        public WhereBetweenOrEqualExpression(IExpression expression, string fieldName, object start, object end)
            : base(expression, fieldName, start, end)
        { }

    }
}
