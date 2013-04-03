using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.EventBus.Content
{
    public class ContentVersioningSubscriber : ISubscriber
    {
        #region ISubscriber Members

        public void Receive(IEventContext context)
        {
            if (context is ContentEventContext)
            {
                var contentContext = ((ContentEventContext)context);
                if (contentContext.Content.___EnableVersion___)
                {
                    if (contentContext.Content is TextContent)
                    {
                        switch (contentContext.ContentAction)
                        {
                            case Kooboo.CMS.Content.Models.ContentAction.Delete:
                                break;
                            case Kooboo.CMS.Content.Models.ContentAction.Add:
                            case Kooboo.CMS.Content.Models.ContentAction.Update:
                                Versioning.VersionManager.LogVersion((TextContent)(contentContext.Content));
                                break;
                            default:

                                break;
                        }
                    }
                }
            }

        }

        #endregion
    }
}
