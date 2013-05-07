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
using System.IO;
using Kooboo.Web.Url;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class Repository
    {
        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        public static Repository Current
        {
            get
            {
                return CallContext.Current.GetObject<Repository>("Current_Repository");
            }
            set
            {
                CallContext.Current.RegisterObject("Current_Repository", value);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        public Repository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Repository(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlIgnore]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [DataMember(Order = 3)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable broadcasting].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable broadcasting]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 9)]
        public bool EnableBroadcasting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable custom template].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enable custom template]; otherwise, <c>false</c>.
        /// </value>
        [Obsolete("Put the custom template into 'CustomTemplates' which is a sub folder of schema.")]
        [DataMember(Order = 11)]
        public bool EnableCustomTemplate { get; set; }

        private string userKeyReplacePattern;
        /// <summary>
        /// Gets or sets the user key replace pattern.
        /// </summary>
        [DataMember(Order = 15)]
        public string UserKeyReplacePattern
        {
            get
            {
                if (string.IsNullOrEmpty(userKeyReplacePattern))
                {
                    return "[^\\w\\d_-]+";
                }
                return userKeyReplacePattern;
            }
            set
            {
                if (value != "[ ;@=$,#%&*{}\\:<>?/+\"\'!|]{1,}")
                {
                    userKeyReplacePattern = value;
                }

            }
        }
        private string userKeyHyphens;
        /// <summary>
        /// Gets or sets the user key hyphens.
        /// </summary>
        [DataMember(Order = 17)]
        public string UserKeyHyphens
        {
            get
            {
                if (string.IsNullOrEmpty(userKeyHyphens))
                {
                    return "-";
                }
                return userKeyHyphens;
            }
            set
            {
                userKeyHyphens = value;
            }
        }
        private bool? enableVersioning = true;
        /// <summary>
        /// Gets or sets the enable versioning.
        /// </summary>
        [DataMember(Order = 19)]
        public bool? EnableVersioning
        {
            get
            {
                if (enableVersioning == null)
                {
                    enableVersioning = true;
                }
                return enableVersioning;
            }
            set
            {
                enableVersioning = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable workflow].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable workflow]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20)]
        public bool EnableWorkflow { get; set; }

        /// <summary>
        /// 是否使用相对严格的权限模式，就是当目录没有指定权限设置时，非管理员用户无法看到
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [strict content permission]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool StrictContentPermission { get; set; }

        private bool _showHiddenFolders = false;
        [DataMember]
        public bool ShowHiddenFolders
        {
            get { return _showHiddenFolders; }
            set
            {
                _showHiddenFolders = value;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class Repository : IPersistable, IIdentifiable
    {
        #region IPersistable Members
        public string UUID
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            isDummy = false;
            this.Name = ((Repository)source).Name;
        }

        void IPersistable.OnSaved()
        {

        }

        void IPersistable.OnSaving()
        {

        }

        #endregion
        #region override object
        public static bool operator ==(Repository obj1, Repository obj2)
        {
            if (object.Equals(obj1, obj2) == true)
            {
                return true;
            }
            if (object.Equals(obj1, null) == true || object.Equals(obj2, null) == true)
            {
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(Repository obj1, Repository obj2)
        {
            return !(obj1 == obj2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Repository))
            {
                return false;
            }
            if (obj != null)
            {
                var repository = (Repository)obj;
                if (string.Compare(this.Name, repository.Name, true) == 0)
                {
                    return true;
                }
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return this.Name;
        }
        #endregion
    }


    /// <summary>
    /// 
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Creates the transaction.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <returns></returns>
        public static ITransactionUnit CreateTransaction(this Repository repository)
        {
            return Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>().CreateTransaction(repository);
        }
    }
}
