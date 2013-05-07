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

using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Models
{
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
