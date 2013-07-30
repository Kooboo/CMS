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

namespace Kooboo
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class KoobooException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KoobooException" /> class.
        /// </summary>
        public KoobooException()
            : base()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="KoobooException" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public KoobooException(string msg)
            : base(msg)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="KoobooException" /> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="exception">The exception.</param>
        public KoobooException(string msg, Exception exception)
            : base(msg, exception)
        { }
    }
}
