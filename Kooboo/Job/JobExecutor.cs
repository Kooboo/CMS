#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Threading;

namespace Kooboo.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class JobExecutor
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="JobExecutor" /> class.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="executionState">State of the execution.</param>
        public JobExecutor(IJob job, int interval, object executionState)
        {
            this.Job = job;
            this.Interval = interval;
            this.ExecutionState = executionState;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        public IJob Job { get; private set; }
        /// <summary>
        /// Gets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public int Interval { get; private set; }
        /// <summary>
        /// Gets the state of the execution.
        /// </summary>
        /// <value>
        /// The state of the execution.
        /// </value>
        public object ExecutionState { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="JobExecutor" /> is started.
        /// </summary>
        /// <value>
        ///   <c>true</c> if started; otherwise, <c>false</c>.
        /// </value>
        public bool Started { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning { get; private set; }
        #endregion

        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private Timer timer;
        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (Started)
            {
                return;
            }
            timer = new Timer(new TimerCallback(TimerCallback), ExecutionState, Interval, Interval);
            Started = true;
        }
        private void TimerCallback(object state)
        {
            if (!Started || IsRunning)
            {
                return;
            }

            timer.Change(Timeout.Infinite, Timeout.Infinite);

            try
            {
                Job.Execute(state);
            }
            catch (Exception e)
            {
                Job.Error(e);
            }

            IsRunning = false;

            if (Started)
            {
                timer.Change(Interval, Interval);
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            Started = false;
            timer.Dispose();
            timer = null;
        }
        #endregion
    }
}
