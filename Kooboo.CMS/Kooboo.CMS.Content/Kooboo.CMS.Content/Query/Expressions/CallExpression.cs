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
    public enum CallType
    {
        Unspecified,
        Count,
        First,
        Last,
        LastOrDefault,
        FirstOrDefault        
    }
    public class CallExpression : Expression
    {
        public CallExpression(IExpression expression, CallType type)
            : base(expression)
        {
            this.CallType = type;
        }
        public CallType CallType { get; private set; }
    }
}
