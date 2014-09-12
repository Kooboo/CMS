#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Models
{
    #region Persistence
    [DataContract]
    public partial class RemoteTextFolderMapping
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string LocalFolderId { get; set; }

        [DataMember]
        public string RemoteEndpoint { get; set; }

        [DataMember]
        public string RemoteFolderId { get; set; }

        [DataMember]
        public bool Enabled { get; set; }
    }
    #endregion

    public partial class RemoteTextFolderMapping : IPersistable, IIdentifiable, ISiteObject
    {
        public RemoteTextFolderMapping()
        {

        }

        public RemoteTextFolderMapping(Site site, string uuid)
        {
            this.Site = site;
            this.UUID = uuid;
        }

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

        private bool _isDummy = true;
        public bool IsDummy
        {
            get
            {
                return this._isDummy;
            }
            private set
            {
                this._isDummy = value;
            }
        }

        public void Init(IPersistable source)
        {
            this.IsDummy = false;
            this.Site = ((ISiteObject)source).Site;
        }

        public void OnSaved()
        {
            this.IsDummy = false;
        }

        public void OnSaving()
        {

        }
        public Site Site { get; set; }
    }
}
