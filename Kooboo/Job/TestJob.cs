using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kooboo.Job
{
    public class TestJob : IJob
    {
        string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testjob.txt");
        #region IJob Members

        public void Execute(object executionState)
        {
            System.Threading.Thread.Sleep(3000);
            File.AppendAllLines(logFile, new[] { string.Format("Run on {0}", DateTime.Now) });
        }

        public void Error(Exception e)
        {
            File.AppendAllLines(logFile, new[] { string.Format("Exception throwed on {0}, message:{1}", DateTime.Now, e.Message) });
        }

        #endregion
    }
}
