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
    public class WhereClauseExpression : Expression, IWhereExpression
    {
        public WhereClauseExpression(string whereClause)
            : this(null, whereClause)
        {

        }
        public WhereClauseExpression(IExpression expression, string whereClause)
            : base(expression)
        {
            this.WhereClause = whereClause;
        }
        public string WhereClause { get; private set; }
    }
}
