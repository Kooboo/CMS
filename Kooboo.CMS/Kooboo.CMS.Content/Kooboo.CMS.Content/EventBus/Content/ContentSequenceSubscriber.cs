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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Content.EventBus.Content
{
    public class ContentSequenceSubscriber : ISubscriber
    {
        public EventResult Receive(IEventContext context)
        {
            EventResult eventResult = new EventResult();
            if (context is ContentEventContext)
            {
                try
                {
                    var contentEventContext = (ContentEventContext)context;
                    switch (contentEventContext.ContentAction)
                    {
                        case Kooboo.CMS.Content.Models.ContentAction.Add:
                            break;
                        case Kooboo.CMS.Content.Models.ContentAction.Update:
                            break;
                        case Kooboo.CMS.Content.Models.ContentAction.Delete:
                            break;
                        case Kooboo.CMS.Content.Models.ContentAction.PreAdd:
                            if (contentEventContext.Content.Sequence == 0)
                            {
                                var textFolder = contentEventContext.Content.GetFolder().AsActual();
                                int sequence = 1;
                                var maxSequenceContent = textFolder.CreateQuery().OrderByDescending("Sequence").FirstOrDefault();
                                if (maxSequenceContent != null)
                                {
                                    sequence = maxSequenceContent.Sequence + 1;
                                }
                                contentEventContext.Content.Sequence = sequence;

                            }

                            break;
                        case Kooboo.CMS.Content.Models.ContentAction.PreUpdate:
                            break;
                        case Kooboo.CMS.Content.Models.ContentAction.PreDelete:
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    eventResult.Exception = e;
                }
                
            }
            return eventResult;
        }
    }
}
