using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public class WhereBetweenExpression : WhereFieldExpression
    {
        public WhereBetweenExpression(IExpression expression, string fieldName, object start, object end)
            : base(expression, fieldName)
        {
            this.Start = start;
            this.End = end;
        }
        public object Start { get; set; }
        public object End { get; private set; }
    }
}
