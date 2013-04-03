using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Dynamic;
using Kooboo.Extensions;
using Kooboo.Web.Url;
using System.IO;
using Kooboo.CMS.Content.Models.Paths;
namespace Kooboo.CMS.Content.Models
{

    /// <summary>
    /// 什么类型的内容
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown,
        /// <summary>
        /// 文本内容
        /// </summary>
        Text,
        /// <summary>
        /// media二进制文件
        /// </summary>
        Media
    }

    /// <summary>
    /// 内容上传的附件
    /// </summary>
    public class ContentFile
    {
        /// <summary>
        /// 附件对应的字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 附件的文件名
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public Stream Stream { get; set; }
    }
    /// <summary>
    /// 内容对象基类
    /// </summary>
    public partial class ContentBase : DynamicDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBase"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public ContentBase(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
            this.Id = string.Empty;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBase"/> class.
        /// </summary>
        public ContentBase()
        {
            this.UserKey = "";
            this.Id = string.Empty;

            this.UtcCreationDate = DateTime.UtcNow;
            this.UtcLastModificationDate = DateTime.UtcNow;

            this.UUID = UUIDGenerator.DefaultGenerator.Generate(this);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBase"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="folderName">Name of the folder.</param>
        public ContentBase(string repository, string folderName)
            : this()
        {
            this.Repository = repository;
            this.FolderName = folderName;
            this.Id = string.Empty;

            this.UtcCreationDate = DateTime.UtcNow;
            this.UtcLastModificationDate = DateTime.UtcNow;
        }
        /// <summary>
        /// 在关系型数据库中，这是一个自增长Id值，
        /// 在非关系型数据库中，这个值可能为空
        /// </summary>
        public string Id
        {
            get
            {
                if (this.ContainsKey("Id") && base["Id"] != null)
                {
                    return base["Id"].ToString();
                }
                return string.Empty;
            }
            set
            {
                base["Id"] = value;
            }
        }
        /// <summary>
        /// 内容的唯一主键
        /// </summary>
        public string UUID
        {
            get
            {
                if (this.ContainsKey("UUID") && base["UUID"] != null)
                {
                    return base["UUID"].ToString();
                }
                return string.Empty;
            }
            set
            {
                base["UUID"] = value;
            }
        }
        /// <summary>
        /// 内容对应的仓库名称
        /// </summary>
        public string Repository
        {
            get
            {
                if (this.ContainsKey("Repository") && base["Repository"] != null)
                {
                    return base["Repository"].ToString();
                }
                return null;
            }
            set
            {
                base["Repository"] = value;
            }
        }

        /// <summary>
        /// 内容所在的目录名称
        /// </summary>
        /// <value>
        /// The name of the folder.
        /// </value>
        public string FolderName
        {
            get
            {
                if (this.ContainsKey("FolderName") && base["FolderName"] != null)
                {
                    return base["FolderName"].ToString();
                }
                return null;
            }
            set
            {
                base["FolderName"] = value;
            }
        }

        /// <summary>
        /// 根据Summary字段生成的一个友好的唯一主键值，一般用于在URL中传递。
        /// </summary>
        public string UserKey
        {
            get
            {
                if (this.ContainsKey("UserKey") && base["UserKey"] != null)
                {
                    return base["UserKey"] == null ? "" : base["UserKey"].ToString();
                }
                return null;
            }
            set
            {
                base["UserKey"] = value;
            }
        }

        /// <summary>
        /// 内容的创建时间，UTC时间
        /// </summary>
        public DateTime UtcCreationDate
        {
            get
            {
                if (this.ContainsKey("UtcCreationDate") && base["UtcCreationDate"] != null && base["UtcCreationDate"] is DateTime)
                {
                    return (DateTime)base["UtcCreationDate"];
                }
                return DateTime.UtcNow;
            }
            set
            {
                base["UtcCreationDate"] = value;
            }
        }

        /// <summary>
        /// 内容的最后修改时间，UTC时间
        /// </summary>
        public DateTime UtcLastModificationDate
        {
            get
            {
                if (this.ContainsKey("UtcLastModificationDate") && base["UtcLastModificationDate"] != null && base["UtcLastModificationDate"] is DateTime)
                {
                    return (DateTime)base["UtcLastModificationDate"];
                }
                return DateTime.UtcNow;
            }
            set
            {
                base["UtcLastModificationDate"] = value;
            }
        }

        /// <summary>
        ///内容的发布状态
        /// </summary>
        public bool? Published
        {
            get
            {
                if (this.ContainsKey("Published") && base["Published"] != null)
                {
                    return (bool)base["Published"];
                }
                return null;
            }
            set
            {
                base["Published"] = value;
            }
        }

        /// <summary>
        /// 维护内容的用户ID
        /// </summary>
        public string UserId
        {
            get
            {
                if (this.ContainsKey("UserId") && base["UserId"] != null)
                {
                    return base["UserId"] == null ? "" : base["UserId"].ToString();
                }
                return null;
            }
            set
            {
                base["UserId"] = value;
            }
        }

        public virtual ContentType ContentType
        {
            get
            {
                return Models.ContentType.Unknown;
            }
        }
    }

    /// <summary>
    /// ContentBase对IPersistable的实现
    /// </summary>
    public partial class ContentBase : IPersistable
    {
        #region IPersistable Members

        private bool isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            isDummy = false;
        }

        void IPersistable.OnSaved()
        {
            isDummy = false;
        }

        void IPersistable.OnSaving()
        {
            OnSaving();
        }
        protected virtual void OnSaving()
        {
            if (string.IsNullOrEmpty(this.UUID))
            {
                this.UUID = UUIDGenerator.DefaultGenerator.Generate(this);
            }
        }

        #endregion

    }


    /// <summary>
    /// 文本内容
    /// </summary>
    public class TextContent : ContentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextContent"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public TextContent(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TextContent"/> class.
        /// </summary>
        public TextContent()
            : base()
        {
            this.Id = string.Empty;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TextContent"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="schemaName">Name of the schema.</param>
        /// <param name="folderName">Name of the folder.</param>
        public TextContent(string repository, string schemaName, string folderName)
            : base(repository, folderName)
        {
            this.Id = string.Empty;
            this.SchemaName = schemaName;
        }

        /// <summary>
        /// 内容对应的Schema(Content type)
        /// </summary>
        /// <value>
        /// The name of the schema.
        /// </value>
        public string SchemaName
        {
            get
            {
                if (this.ContainsKey("SchemaName") && base["SchemaName"] != null)
                {
                    return base["SchemaName"].ToString();
                }
                return null;
            }
            set
            {
                base["SchemaName"] = value;
            }
        }

        /// <summary>
        /// 内嵌内容的父目录
        /// </summary>
        public string ParentFolder
        {
            get
            {
                if (this.ContainsKey("ParentFolder") && base["ParentFolder"] != null)
                {
                    return base["ParentFolder"].ToString();
                }
                return null;
            }
            set
            {
                base["ParentFolder"] = value;
            }
        }
        /// <summary>
        ///内嵌内容的父内容的UUID
        /// </summary>
        public string ParentUUID
        {
            get
            {
                if (this.ContainsKey("ParentUUID") && base["ParentUUID"] != null)
                {
                    return base["ParentUUID"].ToString();
                }
                return null;
            }
            set
            {
                base["ParentUUID"] = value;
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
                return ContentType.Text;
            }
        }
        /// <summary>
        /// 内容被广播过来的源UUID
        /// </summary>
        public string OriginalUUID
        {
            get
            {
                if (this.ContainsKey("OriginalUUID") && base["OriginalUUID"] != null)
                {
                    return base["OriginalUUID"].ToString();
                }
                return null;
            }
            set
            {
                base["OriginalUUID"] = value;
            }
        }
        /// <summary>
        /// 内容被广播过来的源仓库
        /// </summary>
        public string OriginalRepository
        {
            get
            {
                if (this.ContainsKey("OriginalRepository") && base["OriginalRepository"] != null)
                {
                    return base["OriginalRepository"].ToString();
                }
                return null;
            }
            set
            {
                base["OriginalRepository"] = value;
            }
        }
        /// <summary>
        /// 内容被广播过来的源目录
        /// </summary>
        public string OriginalFolder
        {
            get
            {
                if (this.ContainsKey("OriginalFolder") && base["OriginalFolder"] != null)
                {
                    return base["OriginalFolder"].ToString();
                }
                return null;
            }
            set
            {
                base["OriginalFolder"] = value;
            }
        }
        /// <summary>
        ///内容被广播过来后是否本地化了
        /// </summary>
        public bool? IsLocalized
        {
            get
            {
                if (this.ContainsKey("IsLocalized") && base["IsLocalized"] != null)
                {
                    return (bool)base["IsLocalized"];
                }
                return null;
            }
            set
            {
                base["IsLocalized"] = value;
            }
        }

        /// <summary>
        /// 内容的排序顺序
        /// </summary>
        public int Sequence
        {
            get
            {
                if (this.ContainsKey("Sequence") && base["Sequence"] != null)
                {
                    return Convert.ToInt32(base["Sequence"]);
                }
                return 0;
            }
            set
            {
                base["Sequence"] = value;
            }
        }

        /// <summary>
        /// 内容是否有附件
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has attachment; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAttachment()
        {
            var schema = this.GetSchema();
            foreach (var column in schema.AsActual().Columns.Where(it => string.Compare(it.ControlType, "File", true) == 0))
            {
                var value = this[column.Name];
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 保存时，要存储的附件
        /// </summary>
        public IEnumerable<ContentFile> ContentFiles { get; set; }

        /// <summary>
        /// Called when [saving].
        /// </summary>
        protected override void OnSaving()
        {
            base.OnSaving();
            this.UserKey = UserKeyGenerator.DefaultGenerator.Generate(this);
        }


        #region override object
        public override bool Equals(object obj)
        {
            if (!(obj is ContentBase))
            {
                return false;
            }
            var c = (ContentBase)obj;
            if (this.UUID.EqualsOrNullEmpty(c.UUID, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        public bool ___EnableVersion___
        {
            get
            {
                if (this.ContainsKey("___EnableVersion___") && base["___EnableVersion___"] != null)
                {
                    return (bool)base["___EnableVersion___"];
                }
                return true;
            }
            set
            {
                base["___EnableVersion___"] = value;
            }
        }
    }   
}
