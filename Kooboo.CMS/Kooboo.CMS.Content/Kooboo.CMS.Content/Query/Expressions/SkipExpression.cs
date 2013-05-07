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
    public class SkipExpression : Expression
    {
        public SkipExpression(IExpression expression, int count)
            : base(expression)
        {
            this.Count = count;
        }
        public int Count { get; private set; }
    }
}
