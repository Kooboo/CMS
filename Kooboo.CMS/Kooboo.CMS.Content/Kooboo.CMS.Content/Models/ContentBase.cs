#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Content.Models
{
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
        /// <summary>
        /// The content integrate id composite of Repository, FolderName and UUID. example: Repository#FolderName#UUID
        /// </summary>
        public string IntegrateId
        {
            get
            {
                return new ContentIntegrateId(this).ToString();
            }
        }
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

}
