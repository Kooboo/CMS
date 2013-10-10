using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.Globalization;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// the content integrate id composite of Repository, FolderName and UUID.
    /// </summary>
    public class ContentIntegrateId : IntegrateIdBase
    {
        public string Repository { get; private set; }
        public string FolderName { get; private set; }
        public string ContentUUID { get; private set; }
        #region .ctor
        public ContentIntegrateId(ContentBase content)
            : this(content.Repository, content.FolderName, content.UUID)
        {

        }
        public ContentIntegrateId(string repository, string folderName, string uuid)
        {
            base.Segments = new[] { repository, folderName, uuid };
        }

        public ContentIntegrateId(string id)
            : base(id)
        {
            if (base.Segments == null || base.Segments.Length < 3)
            {
                throw new ArgumentException("Invalid content integrate id.");
            }

            Repository = this.Segments[0];
            FolderName = this.Segments[1];
            ContentUUID = this.Segments[2];
        }
        #endregion
    }
}
