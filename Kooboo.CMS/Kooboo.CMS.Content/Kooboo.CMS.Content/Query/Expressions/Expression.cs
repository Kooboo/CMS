using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public abstract class Expression : IExpression
    {
        public Expression(IExpression expression)
        {
            this.InnerExpression = expression;
        }
        public IExpression InnerExpression { get; private set; }
    }
}
