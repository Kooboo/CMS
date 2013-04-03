using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public class WhereCategoryExpression : Expression, IWhereExpression
    {
        public WhereCategoryExpression(IExpression expression, IContentQuery<TextContent> categoryQuery)
            : base(expression)
        {
            this.CategoryQuery = categoryQuery;
        }
        public IContentQuery<TextContent> CategoryQuery { get; private set; }
    }
}
