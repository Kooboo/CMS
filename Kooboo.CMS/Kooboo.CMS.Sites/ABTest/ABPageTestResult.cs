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

namespace Kooboo.CMS.Sites.ABTest
{
    public class ABPageTestHits
    {
        public string PageName { get; set; }
        public int ShowTimes { get; set; }
        public int HitTimes { get; set; }
    }
    [DataContract]
    public partial class ABPageTestResult
    {
        [DataMember]
        public string ABPageUUID { get; set; }

        [Obsolete("Use ABPageUUID")]
        [DataMember]
        public string PageVisitRuleUUID { get { return ABPageUUID; } set { this.ABPageUUID = value; } }
        [DataMember]
        public List<ABPageTestHits> PageHits { get; set; }
        [DataMember]
        public int TotalShowTimes { get; set; }
        [DataMember]
        public int TotalHitTimes { get; set; }
    }

    public partial class ABPageTestResult : ISiteObject, IPersistable, IIdentifiable
    {
        #region IPersistable Members
        private bool isDummy = true;
        bool Common.Persistence.Non_Relational.IPersistable.IsDummy
        {
            get { return isDummy; }
        }

        void Common.Persistence.Non_Relational.IPersistable.Init(Common.Persistence.Non_Relational.IPersistable source)
        {

        }

        void Common.Persistence.Non_Relational.IPersistable.OnSaved()
        {
            isDummy = false;
        }

        void Common.Persistence.Non_Relational.IPersistable.OnSaving()
        {

        }
        #endregion

        #region IIdentifiable
        public string UUID
        {
            get
            {
                return this.ABPageUUID;
            }
            set
            {
                this.ABPageUUID = value;
            }
        }
        #endregion

        #region ISiteObject Members
        public Site Site
        {
            get;
            set;
        }
        #endregion
    }
}
