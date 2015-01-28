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
    [KnownType(typeof(Dictionary<string, object>))]
    [KnownType(typeof(Kooboo.CMS.Sites.Models.Page))]
    public partial class IncomingQueue : IIdentifiable
    {
        [DataMember]
        public string UUID { get; set; }
        [DataMember]
        public string Vendor { get; set; }
        [DataMember]
        public PublishingObject PublishingObject { get; set; }
        [DataMember]
        public PublishingAction Action { get; set; }
        [DataMember]
        public string ObjectUUID { get; set; }
        [DataMember]
        public string ObjectTitle { get; set; }
        [DataMember]
        public DateTime UtcCreationDate { get; set; }
        [DataMember]
        public QueueStatus Status { get; set; }
        [DataMember]
        public DateTime? UtcProcessedTime { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public object Object { get; set; }
    }
    #endregion

    public partial class IncomingQueue : IPersistable, ISiteObject
    {
        public IncomingQueue()
        {
            this.UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10);
        }

        public IncomingQueue(Site site, string uuid)
        {
            this.Site = site;
            this.UUID = uuid;
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

        public Site Site
        {
            get;
            set;
        }
    }
}
