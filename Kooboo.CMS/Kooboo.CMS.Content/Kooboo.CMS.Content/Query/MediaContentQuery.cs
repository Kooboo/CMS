using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query.Expressions;

namespace Kooboo.CMS.Content.Query
{
    public static class MediaContentQueryExtensions
    {
        public static IContentQuery<MediaContent> CreateQuery(this MediaFolder binaryFolder)
        {
            return new MediaContentQuery(binaryFolder.Repository, binaryFolder);
        }       
    }
    public class MediaContentQuery : ContentQuery<MediaContent>
    {
        public MediaContentQuery(Repository repository, MediaFolder mediaFolder)
            : this(repository, mediaFolder, null)
        {

        }
        public MediaContentQuery(Repository repository, MediaFolder mediaFolder, IExpression expression)
            : base(repository, expression)
        {
            this.MediaFolder = mediaFolder;
        }
        public MediaFolder MediaFolder { get; private set; }
        public override IContentQuery<MediaContent> Create(IExpression expression)
        {
            return new MediaContentQuery(this.Repository, this.MediaFolder, expression);
        }
    }
}
