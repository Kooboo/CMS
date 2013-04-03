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
            foreach (var s in Subscribers)
            {
                s.Receive(eventContext);
            }
        }
    }
}
