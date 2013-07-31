#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace Kooboo.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum Corner
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        All = 0xFF,
        /// <summary>
        /// 
        /// </summary>
        TopLeft = 0x01,
        /// <summary>
        /// 
        /// </summary>
        TopRight = 0x02,
        /// <summary>
        /// 
        /// </summary>
        BottomLeft = 4,
        /// <summary>
        /// 
        /// </summary>
        BottomRight = 8
    }
}
