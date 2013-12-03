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
using Kooboo.CMS.Content.EventBus;
using Kooboo.CMS.Content.EventBus.Content;
using Kooboo.CMS.Content.Models;
namespace Kooboo.CMS.PluginTemplate
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISubscriber), Key = "CustomContentEventSubscriber")]
    public class CustomContentEventSubscriber : ISubscriber
    {
        EventResult ISubscriber.Receive(IEventContext context)
        {
            if (context is ContentEventContext)
            {
                var contentEventContext = (ContentEventContext)context;
                switch (contentEventContext.ContentAction)
                {
                    case ContentAction.Add:
                        break;
                    case ContentAction.Delete:
                        break;
                    case ContentAction.PreAdd:
                        break;
                    case ContentAction.PreDelete:
                        break;
                    case ContentAction.PreUpdate:
                        break;
                    case ContentAction.Update:
                        break;
                    default:
                        break;
                }
            }

            return new EventResult() { IsCancelled = false };
        }
    }
}
