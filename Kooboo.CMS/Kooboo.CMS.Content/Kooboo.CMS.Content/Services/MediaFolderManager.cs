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
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(MediaFolderManager))]
    public class MediaFolderManager : FolderManager<MediaFolder>
    {
        public MediaFolderManager(IMediaFolderProvider provider)
            : base(provider) { }

        public virtual void Rename(MediaFolder @new, MediaFolder old)
        {
            if (string.IsNullOrEmpty(@new.Name))
            {
                throw new NameIsReqiredException();
            }
            ((IMediaFolderProvider)Provider).Rename(@new, @old);
        }
    }
}
