#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Modules.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence
{
    public interface IRemotePublishingQueueProvider : IPublishingProvider<RemotePublishingQueue>
    {
        IEnumerable<RemotePublishingQueue> GetJobItems(DateTime utcExecutionTime, int maxItems);
    }
}
