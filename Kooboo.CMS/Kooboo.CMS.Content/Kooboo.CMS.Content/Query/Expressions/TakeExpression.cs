using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public class TakeExpression : Expression
    {
        public TakeExpression(IExpression expression, int count)
            : base(expression)
        {
            this.Count = count;
        }
        public int Count { get; set; }
    }
}
