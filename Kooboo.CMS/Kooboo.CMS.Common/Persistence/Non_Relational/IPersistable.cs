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

namespace Kooboo.CMS.Common.Persistence.Non_Relational
{
    /// <summary>
    /// 可持久化的对象接口    
    /// </summary>
    public interface IPersistable
    {
        /// <summary>
        /// 是否为不完整对象
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dummy; otherwise, <c>false</c>.
        /// </value>
        bool IsDummy { get; }
        /// <summary>
        /// 在反序列化对象后会调用的初始化方法
        /// </summary>
        /// <param name="source">The source.</param>
        void Init(IPersistable source);
        /// <summary>
        /// Called when [saved].
        /// </summary>
        void OnSaved();
        /// <summary>
        /// Called when [saving].
        /// </summary>
        void OnSaving();
    }
}
