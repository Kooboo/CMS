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

namespace Kooboo.CMS.Sites.ABTest
{
    public class ABSiteRuleItem
    {
        public string RuleItemName { get; set; }
        public string SiteName { get; set; }
    }
    [DataContract]
    public partial class ABSiteSetting
    {
        [DataMember]
        public string MainSite { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public List<ABSiteRuleItem> Items { get; set; }
    }
    public partial class ABSiteSetting : IPersistable, IIdentifiable
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
                return this.MainSite;
            }
            set
            {
                this.MainSite = value;
            }
        }
        #endregion
    }
}
