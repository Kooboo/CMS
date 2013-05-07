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
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.ABTest
{
    [DataContract]
    public partial class ABRuleSetting
    {
        #region .ctor
        public ABRuleSetting()
        {
            this.RuleItems = new List<IVisitRule>();
        }
        public ABRuleSetting(Site site, string name)
            : this()
        {
            this.Site = site;
            this.Name = name;
        }
        #endregion
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string RuleType { get; set; }
        [DataMember]
        public List<IVisitRule> RuleItems
        {
            get;
            set;
        }
    }
    public partial class ABRuleSetting : ISiteObject, IPersistable, IIdentifiable
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
            this.Site = ((ABRuleSetting)source).Site;
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
