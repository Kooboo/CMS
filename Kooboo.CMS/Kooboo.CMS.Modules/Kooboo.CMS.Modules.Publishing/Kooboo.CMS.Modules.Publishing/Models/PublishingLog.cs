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
    public partial class PublishingLog : ISiteObject, IIdentifiable
    {
        [DataMember]
        public string UUID { get; set; }
        [DataMember]
        public QueueType QueueType { get; set; }
        [DataMember]
        public string QueueUUID { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public PublishingObject PublishingObject { get; set; }
        [DataMember]
        public string ObjectUUID { get; set; }
        [DataMember]
        public string ObjectTitle { get; set; }
        //[DataMember]
        //public PublishingType PublishingType { get; set; }
        [DataMember]
        public PublishingAction PublishingAction { get; set; }
        [DataMember]
        //Use for publishing page
        public string RemoteEndpoint { get; set; }
        [DataMember]
        //Use for publishing text content.
        public string TextFolderMapping { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public bool? PublishDraft { get; set; }
        [DataMember]
        public QueueStatus Status { get; set; }
        [DataMember]
        public string Vendor { get; set; }
        [DataMember]
        public DateTime? UtcProcessedTime { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string StackTrace { get; set; }
        //[DataMember]
        public object QueueObject { get; set; }
    }
    #endregion

    public partial class PublishingLog : IPersistable
    {
        public PublishingLog()
        {
            this.UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(12);
        }

        public PublishingLog(string uuid)
        {
            this.UUID = uuid;
        }

        private bool _isDummy = true;
        public bool IsDummy
        {
            get
            {
                return this._isDummy;
            }
            set
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
