#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common.Persistence.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Models
{
    #region Persistence
    [DataContract]
    public partial class RemoteTextFolderMapping : ISiteObject
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SiteName { get; set; }

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

    public partial class RemoteTextFolderMapping : IPersistable, IIdentifiable
    {
        public RemoteTextFolderMapping()
        {
            
        }

        public RemoteTextFolderMapping(string uuid)
        {
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
        }

        public void OnSaved()
        {
            this.IsDummy = false;
        }

        public void OnSaving()
        {

        }
    }
}
