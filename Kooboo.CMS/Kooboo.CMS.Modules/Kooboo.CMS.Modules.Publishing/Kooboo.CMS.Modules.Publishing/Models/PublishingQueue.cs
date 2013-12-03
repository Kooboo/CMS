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
    public partial class PublishingQueue : IIdentifiable, ISiteObject
    {
        [DataMember]
        public string UUID { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public PublishingObject PublishingObject { get; set; }
        [DataMember]
        public string ObjectUUID { get; set; }
        [DataMember]
        public PublishingType PublishingType { get; set; }
        [DataMember]
        public DateTime? UtcTimeToPublish { get; set; }
        [DataMember]
        public DateTime? UtcTimeToUnpublish { get; set; }
        [DataMember]
        //Use for publishing page
        public string[] RemoteEndpoints { get; set; }
        [DataMember]
        //Use for publishing text content.
        public string[] PublishingMappings { get; set; }
        [DataMember]
        public DateTime UtcCreationDate { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public bool PublishDraft { get; set; }
        [DataMember]
        public QueueStatus Status { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime? UtcProcessedTime { get; set; }
    }
    #endregion

    public partial class PublishingQueue : IPersistable
    {
        public PublishingQueue()
        {
            this.UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10);
        }

        public PublishingQueue(string uuid)
        {
            this.UUID = uuid;
        }

        private bool _isDummy = true;
        public bool IsDummy
        {
            get
            {
                return _isDummy;
            }
            private set
            {
                _isDummy = value;
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
