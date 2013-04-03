using Kooboo.Drawing;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Models
{
    public class MediaContentMetadata
    {

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the alternate text.
        /// Alt for the image tag.
        /// </summary>
        /// <value>
        /// The alternate text.
        /// </value>
        public string AlternateText
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get;
            set;
        }
    }
    /// <summary>
    /// Media 二进制文件内容
    /// </summary>
    public class MediaContent : ContentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaContent"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public MediaContent(IDictionary<string, object> dictionary)
            : base(dictionary)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaContent"/> class.
        /// </summary>
        public MediaContent()
            : base()
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaContent"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="folderName">Name of the folder.</param>
        public MediaContent(string repository, string folderName)
            : base(repository, folderName)
        {

        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName
        {
            get
            {
                if (this.ContainsKey("FileName") && base["FileName"] != null)
                {
                    return (string)base["FileName"];
                }
                return null;
            }
            set
            {
                base["FileName"] = value;
            }

        }

        /// <summary>
        /// Gets or sets the virtual path.
        /// </summary>
        public string VirtualPath
        {
            get
            {
                if (this.ContainsKey("VirtualPath") && base["VirtualPath"] != null)
                {
                    return (string)base["VirtualPath"];
                }
                return null;
            }
            set
            {
                base["VirtualPath"] = value;
            }
        }

        /// <summary>
        /// Gets the physical path.
        /// </summary>
        public string PhysicalPath
        {
            get
            {
                return UrlUtility.MapPath(Uri.UnescapeDataString(this.VirtualPath));
            }
        }
        /// <summary>
        /// Gets the URL.
        /// </summary>
        public string Url
        {
            get
            {
                return UrlUtility.ResolveUrl(this.VirtualPath);
            }
        }
        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public override ContentType ContentType
        {
            get
            {
                return ContentType.Media;
            }
        }

        /// <summary>
        /// Gets or sets the content file.
        /// </summary>
        public ContentFile ContentFile { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// Gets the type of the file.
        /// </summary>
        /// <value>
        /// The type of the file.
        /// </value>
        public string FileType
        {
            get
            {
                var extension = Path.GetExtension(this.FileName);
                if (string.IsNullOrEmpty(extension))
                {
                    return "Unknow";
                }
                return extension.Substring(extension.LastIndexOf(".") + 1);
            }
        }
        public bool IsImage
        {
            get
            {
                return ImageTools.IsImageExtension(Path.GetExtension(this.FileName));
            }
        }
        public MediaContentMetadata Metadata { get; set; }
    }
}
