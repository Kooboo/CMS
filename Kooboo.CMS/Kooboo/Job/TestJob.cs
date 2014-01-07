#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.IO;

namespace Kooboo.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class TestJob : IJob
    {
        #region Fields
        string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testjob.txt");
        #endregion

        #region IJob Members

        /// <summary>
        /// Executes the specified execution state.
        /// </summary>
        /// <param name="executionState">State of the execution.</param>
        public void Execute(object executionState)
        {
            System.Threading.Thread.Sleep(3000);
            File.AppendAllLines(logFile, new[] { string.Format("Run on {0}", DateTime.Now) });
        }

        /// <summary>
        /// Errors the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        public void Error(Exception e)
        {
            File.AppendAllLines(logFile, new[] { string.Format("Exception throwed on {0}, message:{1}", DateTime.Now, e.Message) });
        }

        #endregion
    }
}
