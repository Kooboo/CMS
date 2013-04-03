using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.EventBus;
using Kooboo.CMS.Content.EventBus.Content;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Search
{
    public class ContentEventsSubscriber : ISubscriber
    {
        public void Receive(IEventContext context)
        {
            if (context is ContentEventContext)
            {
                var eventContext = ((ContentEventContext)context);
                if (eventContext.Content is TextContent)
                {
                    try
                    {
                        var textContent = (TextContent)eventContext.Content;
                        var repository = textContent.GetRepository();
                        var searchService = SearchHelper.OpenService(repository);
                        switch (eventContext.ContentAction)
                        {
                            case Kooboo.CMS.Content.Models.ContentAction.Delete:
                                searchService.Delete(textContent);
                                break;
                            case Kooboo.CMS.Content.Models.ContentAction.Add:
                                if (textContent.Published.HasValue && textContent.Published.Value == true)
                                {
                                    searchService.Add(textContent);
                                }
                                break;
                            case Kooboo.CMS.Content.Models.ContentAction.Update:
                                if (textContent.Published.HasValue && textContent.Published.Value == true)
                                {
                                    searchService.Update(textContent);
                                }
                                else
                                {
                                    searchService.Delete(textContent);
                                }

                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Kooboo.HealthMonitoring.Log.LogException(e);
                    }
                }

            }

        }
    }
}
