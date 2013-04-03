using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
namespace Kooboo.CMS.Content.EventBus.Content
{
    public class ContentWorkflowSubscriber : ISubscriber
    {
        public void Receive(IEventContext context)
        {
            if (context is ContentEventContext)
            {
                ContentEventContext contentEventContext = (ContentEventContext)context;
                if (contentEventContext.ContentAction == ContentAction.Add && contentEventContext.Content is TextContent)
                {
                    var textContent = (TextContent)contentEventContext.Content;
                    var folder = (TextFolder)textContent.GetFolder().AsActual();
                    if (folder.EnabledWorkflow && string.IsNullOrEmpty(contentEventContext.Content.UserId))
                    {
                        Services.ServiceFactory.WorkflowManager.StartWorkflow(textContent.GetRepository()
                            , folder.WorkflowName
                            , textContent
                            , contentEventContext.Content.UserId);
                    }
                }
            }
        }
    }
}
