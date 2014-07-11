#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public class BinaryExpression : WhereFieldExpression
    {
        public BinaryExpression(IExpression expression, string fieldName, object value)
            : base(expression, fieldName)
        {
            this.Value = ExpressionValueHelper.Escape(value); 

        }
        public object Value { get; private set; }
    }
}
