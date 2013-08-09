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
using Kooboo.IO;
using Kooboo.CMS.Content.FileServer.Interfaces;
namespace Kooboo.CMS.Content.Persistence.FileServerProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ITextContentFileProvider), Order = 2)]
    public class TextContentFileProvider : ITextContentFileProvider
    {
        public string Save(Models.TextContent content, Models.ContentFile file)
        {
            return RemoteServiceFactory.CreateService<ITextContentFileService>()
                .Add(content.Repository, content.FolderName, content.UUID, file.FileName, file.Stream.ReadData());
        }

        public void DeleteFiles(Models.TextContent content)
        {
            RemoteServiceFactory.CreateService<ITextContentFileService>()
                  .DeleteFiles(content.Repository, content.FolderName, content.UUID);
        }
    }
}
