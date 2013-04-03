using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.Job
{
    public class JobExecutor
    {
        public JobExecutor(IJob job, int interval, object executionState)
        {
            this.Job = job;
            this.Interval = interval;
            this.ExecutionState = executionState;
        }
        public IJob Job { get; private set; }
        public int Interval { get; private set; }
        public object ExecutionState { get; private set; }
        public bool Started { get; set; }
        public bool IsRunning { get; private set; }
        public Timer timer;

        public void Start()
        {
            if (Started)
            {
                return;
            }
            timer = new Timer(new TimerCallback(TimerCallback), ExecutionState, 0, Interval);
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

        public void Stop()
        {
            Started = false;
            timer.Dispose();
            timer = null;
        }
    }
}
