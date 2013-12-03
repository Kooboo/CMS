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

namespace Kooboo.CMS.Modules.Publishing
{
    public class StartScheduleTask : Kooboo.CMS.Common.Runtime.IStartupTask
    {
        public void Execute()
        {
            //one minute
            Job.Jobs.Instance.AttachJob(typeof(Kooboo.CMS.Modules.Publishing.Jobs.ProcessLocalPublishingQueueJob).Name, Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Modules.Publishing.Jobs.ProcessLocalPublishingQueueJob>(), 60000, null);
            Job.Jobs.Instance.AttachJob(typeof(Kooboo.CMS.Modules.Publishing.Jobs.ProcessRemotePublishingQueueJob).Name, Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Modules.Publishing.Jobs.ProcessRemotePublishingQueueJob>(), 60000, null);
            Job.Jobs.Instance.AttachJob(typeof(Kooboo.CMS.Modules.Publishing.Jobs.ProcessIncomeQueueJob).Name, Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Modules.Publishing.Jobs.ProcessIncomeQueueJob>(), 60000, null);

            //Job.Jobs.Instance.AttachJob(typeof(Kooboo.CMS.Modules.Publishing.Jobs.ProcessPublishingQueueJob).Name, Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Modules.Publishing.Jobs.ProcessPublishingQueueJob>(), 60000, null);
            //Job.Jobs.Instance.AttachJob(typeof(Kooboo.CMS.Modules.Publishing.Jobs.ProcessOutgoingQueueJob).Name, Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Modules.Publishing.Jobs.ProcessOutgoingQueueJob>(), 60000, null);
            
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
