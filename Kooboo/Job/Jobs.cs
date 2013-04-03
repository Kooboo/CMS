using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Kooboo.Job
{
    public class Jobs
    {
        private static readonly Jobs instance = new Jobs();

        private Dictionary<string, JobExecutor> jobs = new Dictionary<string, JobExecutor>(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Jobs Instance
        {
            get { return instance; }
        }

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


        static object lockHelper = new object();
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
    }
}
