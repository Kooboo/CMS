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

namespace Kooboo.CMS.Sites.DataSource
{
    [DataContract]
    public partial class DataSourceSetting
    {
        public string DataName { get; set; }
        [DataMember]
        public IDataSource DataSource { get; set; }

        [DataMember]
        public DateTime? UtcCreationDate { get; set; }

        [DataMember]
        public DateTime? UtcLastestModificationDate { get; set; }

        [DataMember]
        public string LastestEditor { get; set; }
    }

    public partial class DataSourceSetting : ISiteObject, IPersistable, IIdentifiable, IInheritable<DataSourceSetting>
    {
        #region .ctor
        public DataSourceSetting()
        {

        }
        public DataSourceSetting(Site site, string uuid)
        {
            this.Site = site;
            this.UUID = uuid;
        }
        #endregion

        #region ISiteObject
        public Site Site
        {
            get;
            set;
        }
        #endregion

        #region IIdentifiable
        public string UUID
        {
            get
            {
                return this.DataName;
            }
            set
            {
                this.DataName = value;
            }
        }
        #endregion

        #region IPersistable
        private bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            private set { this.isDummy = value; }
        }

        void IPersistable.Init(IPersistable source)
        {
            this.IsDummy = false;
            var setting = (DataSourceSetting)source;

            this.UUID = setting.UUID;
            this.Site = setting.Site;
        }
        void IPersistable.OnSaved()
        {
            this.IsDummy = false;
        }
        public virtual void OnSaving()
        {

        }

        #endregion

        #region IInheritable<DataSourceSetting> Members
        public DataSourceSetting LastVersion()
        {
            return LastVersion(this.Site);
        }
        public DataSourceSetting LastVersion(Site site)
        {
            var lastVersion = new DataSourceSetting(site, this.UUID);
            while (!lastVersion.Exists())
            {
                if (lastVersion.Site.Parent == null)
                {
                    break;
                }
                lastVersion = new DataSourceSetting(lastVersion.Site.Parent, this.UUID);
            }
            return lastVersion;
        }

        public bool IsLocalized(Site site)
        {
            return this.Site.Equals(site);
        }

        public bool HasParentVersion()
        {
            var parentSite = this.Site.Parent;
            while (parentSite != null)
            {
                var layout = new DataSourceSetting(parentSite, this.UUID);
                if (layout.Exists())
                {
                    return true;
                }
                parentSite = parentSite.Parent;
            }
            return false;
        }

        #endregion

        #region Exists
        public bool Exists()
        {
            return this.AsActual() != null;
        }
        #endregion

    }

}
