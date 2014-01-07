#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace Kooboo.Dynamic.Calculator
{
    /// <summary>
    /// 
    /// </summary>
    public class CalculateExpression : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculateExpression" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public CalculateExpression(string msg)
            : base(msg)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculateExpression" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="inner">The inner.</param>
        public CalculateExpression(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}
