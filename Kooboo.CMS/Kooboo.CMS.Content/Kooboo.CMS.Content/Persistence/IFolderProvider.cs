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
using System.IO;

namespace Kooboo.CMS.Content.Persistence
{
    public interface IFolderProvider<T> : IContentElementProvider<T>
        where T : Folder
    {
        IQueryable<T> ChildFolders(T parent);
        void Export(Repository repository, IEnumerable<T> models, System.IO.Stream outputStream);
        void Import(Repository repository, T folder, System.IO.Stream zipStream, bool @override);
    }

    public interface ITextFolderProvider : IFolderProvider<TextFolder>
    {
        IQueryable<TextFolder> BySchema(Schema schema);
    }
    public interface IMediaFolderProvider : IFolderProvider<MediaFolder>
    {
        void Rename(MediaFolder @new, MediaFolder old);
        void Export(Repository repository, string baseFolder, string[] folders, string[] docs, Stream outputStream);
    }
}
