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
using System.Threading.Tasks;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Content.EventBus.Content
{
    /// <summary>
    /// Delete the child contents when the content deleted.
    /// </summary>
    public class CascadingContentDeletingSubscriber : ISubscriber
    {
        public EventResult Receive(IEventContext context)
        {
            EventResult eventResult = new EventResult();

            if (context is ContentEventContext)
            {
                var contentEventContext = (ContentEventContext)context;
                if (contentEventContext.ContentAction == Models.ContentAction.Delete)
                {
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var textFolder = contentEventContext.Content.GetFolder().AsActual();

                            // Delete the child contents in this folder.
                            DeleteChildContents(contentEventContext.Content, textFolder, textFolder);

                            if (textFolder.EmbeddedFolders != null)
                            {
                                foreach (var folderName in textFolder.EmbeddedFolders)
                                {
                                    var childFolder = new TextFolder(textFolder.Repository, folderName);
                                    DeleteChildContents(contentEventContext.Content, textFolder, childFolder);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            eventResult.Exception = e;
                        }

                    });
                }
            }

            return eventResult;
        }

        private static void DeleteChildContents(TextContent textContent, TextFolder parentFolder, TextFolder childFolder)
        {
            var repository = textContent.GetRepository();
            var childContents = childFolder.CreateQuery().WhereEquals("ParentFolder", parentFolder.FullName)
                .WhereEquals("ParentUUID", textContent.UUID);
            foreach (var content in childContents)
            {
                Services.ServiceFactory.TextContentManager.Delete(repository, childFolder, content.UUID);
            }
        }
    }
}
