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
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.ABTest
{
    public class ABPageRuleItem
    {
        public string RuleItemName { get; set; }
        public string PageName { get; set; }
    }
    [DataContract]
    public partial class ABPageSetting
    {
        [DataMember]
        public string MainPage { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public List<ABPageRuleItem> Items { get; set; }
        [DataMember]
        public string ABTestGoalPage { get; set; }
    }
    public partial class ABPageSetting : ISiteObject, IPersistable, IIdentifiable
    {
        #region IPersistable Members
        private bool isDummy = true;
        bool Common.Persistence.Non_Relational.IPersistable.IsDummy
        {
            get { return isDummy; }
        }

        void Common.Persistence.Non_Relational.IPersistable.Init(Common.Persistence.Non_Relational.IPersistable source)
        {
            isDummy = false;
            this.Site = ((ABPageSetting)source).Site;
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
                return this.MainPage;
            }
            set
            {
                this.MainPage = value;
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
