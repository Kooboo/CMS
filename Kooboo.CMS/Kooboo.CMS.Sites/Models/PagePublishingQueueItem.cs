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
using System.Runtime.Serialization;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Models
{
    [DataContract]
    public partial class PagePublishingQueueItem
    {
        public PagePublishingQueueItem()
        {

        }
        public PagePublishingQueueItem(Site site, string pageName)
        {
            this.Site = site;
            this.PageName = pageName;
            this.IsDummy = true;
        }

        public string PageName { get; set; }

        [DataMember(Order = 1)]
        public bool PublishDraft { get; set; }

        [DataMember(Order = 2)]
        public DateTime CreationUtcDate { get; set; }

        [DataMember(Order = 4)]
        public DateTime UtcDateToPublish { get; set; }

        [DataMember(Order = 5)]
        public bool Period { get; set; }

        [DataMember(Order = 6)]
        public DateTime UtcDateToOffline { get; set; }
        [DataMember(Order = 7)]
        public string UserName { get; set; }
    }

    public partial class PagePublishingQueueItem : ISiteObject, IFilePersistable, IPersistable, IIdentifiable
    {
        public string UUID
        {
            get
            {
                return this.PageName;
            }
            set
            {
                this.PageName = value;
            }
        }
        public Site Site
        {
            get;
            set;
        }

        public bool IsDummy
        {
            get;
            set;
        }

        public void Init(IPersistable source)
        {
            var o = (PagePublishingQueueItem)source;
            this.Site = o.Site;
            this.PageName = o.PageName;
            this.IsDummy = false;
        }

        public void OnSaved()
        {
            this.IsDummy = false;
        }

        public void OnSaving()
        {

        }
        public static string GetBasePath(Site site)
        {
            return Path.Combine(site.PhysicalPath, "Publishing");
        }
        public string DataFile
        {
            get
            {
                return Path.Combine(GetBasePath(Site), this.PageName + ".config");
            }
        }
    }
}
