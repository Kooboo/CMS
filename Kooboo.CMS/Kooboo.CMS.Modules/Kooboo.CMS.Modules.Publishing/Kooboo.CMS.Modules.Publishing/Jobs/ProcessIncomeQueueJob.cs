#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Modules.Publishing.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Jobs
{
   public  class ProcessIncomeQueueJob: Kooboo.Job.IJob
    {
        #region .ctor
        IIncomingQueueProvider _incomeQueueProvider;
        IncomingQueueManager _incomeQueueManager;
        public ProcessIncomeQueueJob(IIncomingQueueProvider incomeQueueProvider, IncomingQueueManager incomeQueueManager)
        {
            this._incomeQueueProvider = incomeQueueProvider;
            this._incomeQueueManager = incomeQueueManager;
        }
        #endregion

        #region Execute
        public void Execute(object executionState)
        {
            var utcExecutionTime = DateTime.UtcNow;
            var items = _incomeQueueProvider.GetJobItems(20);
            foreach (var item in items)
            {
                _incomeQueueManager.ProcessQueueItem(item);
            }
        }
        #endregion

        public void Error(Exception e)
        {
            Kooboo.HealthMonitoring.Log.LogException(e);
        }
    }
}
