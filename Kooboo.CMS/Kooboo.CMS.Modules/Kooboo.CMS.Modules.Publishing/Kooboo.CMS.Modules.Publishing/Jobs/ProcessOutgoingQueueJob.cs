using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Modules.Publishing.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Jobs
{
    public class ProcessOutgoingQueueJob : Kooboo.Job.IJob
    {
        #region .ctor
        IOutgoingQueueProvider _outgoingQueueProvider;
        OutgoingQueueManager _outgoingQueueManager;
        public ProcessOutgoingQueueJob(IOutgoingQueueProvider outgoingQueueProvider, OutgoingQueueManager outgoingQueueManager)
        {
            this._outgoingQueueProvider = outgoingQueueProvider;
            this._outgoingQueueManager = outgoingQueueManager;
        }
        #endregion

        #region Execute
        public void Execute(object executionState)
        {
            var utcExecutionTime = DateTime.UtcNow;
            var items = _outgoingQueueProvider.GetJobItems(20);
            foreach (var item in items)
            {
                _outgoingQueueManager.ProcessQueueItem(item);
            }
        }
        #endregion

        public void Error(Exception e)
        {
            Kooboo.HealthMonitoring.Log.LogException(e);
        }
    }
}
