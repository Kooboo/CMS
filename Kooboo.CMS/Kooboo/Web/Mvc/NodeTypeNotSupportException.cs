#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Linq.Expressions;

namespace Kooboo.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public class NodeTypeNotSupportException : Exception
    {
        public NodeTypeNotSupportException(ExpressionType expressionType)
            : base(expressionType.ToString() + "is not supported yet.")
        {
        }
    }
}
