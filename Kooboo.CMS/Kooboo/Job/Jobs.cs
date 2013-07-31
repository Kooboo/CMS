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


namespace Kooboo.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class Jobs
    {
        #region Fields
        private static readonly Jobs instance = new Jobs();
        static object lockHelper = new object();
        private Dictionary<string, JobExecutor> jobs = new Dictionary<string, JobExecutor>(StringComparer.CurrentCultureIgnoreCase);
        #endregion

        #region Properties
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Jobs Instance
        {
            get { return instance; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Attaches the job.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="job">The job.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="executionState">State of the execution.</param>
        /// <returns></returns>
        public JobExecutor AttachJob(string name, IJob job, int interval, object executionState)
        {
            if (!jobs.ContainsKey(name))
            {
                lock (lockHelper)
                {
                    if (!jobs.ContainsKey(name))
                    {
                        var jobExecutor = new JobExecutor(job, interval, executionState);
                        jobs[name] = jobExecutor;
                        jobExecutor.Start();
                        return jobExecutor;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            lock (lockHelper)
            {
                foreach (var job in jobs.Values)
                {
                    job.Start();
                }
            }
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            lock (jobs)
            {
                foreach (JobExecutor job in jobs.Values)
                {
                    job.Stop();
                }
            }
        }
        #endregion
    }
}
