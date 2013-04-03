using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence
{
    public interface IFolderProvider<T> : IProvider<T>
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

    }
}
