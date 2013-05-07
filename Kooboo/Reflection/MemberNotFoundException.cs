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

namespace Kooboo.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public class MemberNotFoundException : Exception
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberNotFoundException" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public MemberNotFoundException(string msg)
            : base(msg)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberNotFoundException" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="inner">The inner.</param>
        public MemberNotFoundException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; set; }
        #endregion
    }
}
