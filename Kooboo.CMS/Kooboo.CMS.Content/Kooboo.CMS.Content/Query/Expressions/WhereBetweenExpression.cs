#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public class WhereBetweenExpression : WhereFieldExpression
    {
        public WhereBetweenExpression(string fieldName, object start, object end)
            : this(null, fieldName, start, end)
        { }
        public WhereBetweenExpression(IExpression expression, string fieldName, object start, object end)
            : base(expression, fieldName)
        {
            this.Start = ExpressionValueHelper.Escape(start);
            this.End = ExpressionValueHelper.Escape(end);
        }
        public object Start { get; set; }
        public object End { get; private set; }
    }
}
