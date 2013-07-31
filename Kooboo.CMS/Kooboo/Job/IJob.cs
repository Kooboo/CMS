#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace Kooboo.Job
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// Executes the specified execution state.
        /// </summary>
        /// <param name="executionState">State of the execution.</param>
        void Execute(object executionState);
        /// <summary>
        /// Errors the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        void Error(Exception e);
    }
}
