#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.EventBus
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IEventPublisher))]
    public class EventPublisher : IEventPublisher
    {
        public void Publish<T>(T eventMessage)
        {
            var subscriptions = EngineContext.Current.ResolveAll<ISubscriber<T>>();
            subscriptions.ToList()
                .ForEach(it =>
                {
                    try
                    {
                        it.Handle(eventMessage);
                    }
                    catch (Exception e)
                    {
                        Kooboo.HealthMonitoring.Log.LogException(e);
                    }
                });
        }
    }
}
