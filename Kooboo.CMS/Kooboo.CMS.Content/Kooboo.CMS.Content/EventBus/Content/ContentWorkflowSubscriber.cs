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
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Content.EventBus.Content
{
    public class ContentWorkflowSubscriber : ISubscriber
    {
        public EventResult Receive(IEventContext context)
        {
            EventResult eventResult = new EventResult();
            if (context is ContentEventContext)
            {
                try
                {
                    ContentEventContext contentEventContext = (ContentEventContext)context;
                    if (contentEventContext.ContentAction == ContentAction.Add && contentEventContext.Content is TextContent)
                    {
                        var textContent = (TextContent)contentEventContext.Content;
                        var folder = (TextFolder)textContent.GetFolder().AsActual();
                        if (folder.EnabledWorkflow && !string.IsNullOrEmpty(contentEventContext.Content.UserId))
                        {
                            Services.ServiceFactory.WorkflowManager.StartWorkflow(textContent.GetRepository()
                                , folder.WorkflowName
                                , textContent
                                , contentEventContext.Content.UserId);
                        }
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
