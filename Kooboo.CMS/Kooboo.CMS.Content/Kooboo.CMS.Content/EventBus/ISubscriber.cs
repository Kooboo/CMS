using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.EventBus
{
    public interface ISubscriber
    {
        void Receive(IEventContext context);
    }
}
