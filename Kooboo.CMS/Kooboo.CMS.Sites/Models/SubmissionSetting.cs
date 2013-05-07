#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Models
{
    [DataContract]
    public partial class SubmissionSetting
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string PluginType { get; set; }
        [DataMember]
        public Dictionary<string, string> Settings { get; set; }
    }

    public partial class SubmissionSetting : ISiteObject, IPersistable, IIdentifiable
    {
        #region ISitePersistable
        public Site Site
        {
            get;
            set;
        }

        private bool isDummy = true;
        bool Common.Persistence.Non_Relational.IPersistable.IsDummy
        {
            get { return isDummy; }
        }

        void Common.Persistence.Non_Relational.IPersistable.Init(Common.Persistence.Non_Relational.IPersistable source)
        {
            isDummy = false;
            this.Site = ((SubmissionSetting)source).Site;
        }

        void Common.Persistence.Non_Relational.IPersistable.OnSaved()
        {
            isDummy = false;
        }

        void Common.Persistence.Non_Relational.IPersistable.OnSaving()
        {

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
        #endregion
    }
}
