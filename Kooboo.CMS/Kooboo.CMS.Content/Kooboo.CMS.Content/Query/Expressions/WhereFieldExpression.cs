using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public abstract class WhereFieldExpression : Expression, IWhereExpression
    {
        public WhereFieldExpression(IExpression expression, string fieldName)
            : base(expression)
        {
            this.FieldName = fieldName;
        }
        public string FieldName { get; private set; }

    }
}
