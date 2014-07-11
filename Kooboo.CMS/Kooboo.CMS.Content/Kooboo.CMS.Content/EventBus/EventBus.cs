#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Content.EventBus
{
    public class EventBus
    {
        static EventBus()
        {
            Subscribers = new List<ISubscriber>();
        }
        public static List<ISubscriber> Subscribers { get; set; }

        public static void Send(IEventContext eventContext)
        {
            foreach (var s in ResolveAllSubscribers())
            {
                var eventResult = s.Receive(eventContext);
                if (eventResult != null)
                {
                    if (eventResult.Exception != null)
                    {
                        Kooboo.Common.Logging.Logger.LoggerInstance.Error(eventResult.Exception.Message, eventResult.Exception);
                    }
                    if (eventResult.IsCancelled == true)
                    {
                        break;
                    }
                }
            }
        }
        private static IEnumerable<ISubscriber> ResolveAllSubscribers()
        {
            return Subscribers.Concat(EngineContext.Current.ResolveAll<ISubscriber>());
        }
    }
}
