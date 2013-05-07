#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace Kooboo.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public class MemberException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberException" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public MemberException(string msg)
            : base(msg)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberException" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="inner">The inner.</param>
        public MemberException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}
