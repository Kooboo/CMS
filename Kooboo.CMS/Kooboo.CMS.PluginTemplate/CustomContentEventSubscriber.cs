using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.EventBus;
using Kooboo.CMS.Content.EventBus.Content;
using Kooboo.CMS.Content.Models;
namespace Kooboo.CMS.PluginTemplate
{
    public class CustomContentEventSubscriber : ISubscriber
    {
        public void Receive(IEventContext context)
        {
            if (context is ContentEventContext)
            {
                var eventContext = ((ContentEventContext)context);
                if (eventContext.Content is TextContent)
                {
                    switch (eventContext.ContentAction)
                    {
                        case Kooboo.CMS.Content.Models.ContentAction.Delete:
                            break;
                        case Kooboo.CMS.Content.Models.ContentAction.Add:
                            break;
                        case Kooboo.CMS.Content.Models.ContentAction.Update:
                            break;
                        default:
                            break;
                    }
                }

            }

        }
    }
}
