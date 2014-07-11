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
using System.Threading.Tasks;
using System.Web.Mvc;
using Kooboo.Common.Globalization;
using System.Runtime.Serialization;
namespace Kooboo.CMS.Sites.ABTest
{
    /// <summary>
    /// 
    /// </summary>
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IVisitRule), Kooboo.Common.ObjectContainer.Dependency.ComponentLifeStyle.Transient, Key = "QueryStringVisitRule")]
    [System.Runtime.Serialization.DataContract(Name = "QueryStringVisitRule")]
    [System.Runtime.Serialization.KnownType(typeof(QueryStringVisitRule))]
    public class QueryStringVisitRule : VisitRuleBase, IVisitRule
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string QueryName { get; set; }
        [DataMember]
        public string QueryValue { get; set; }


        public bool IsMatch(System.Web.HttpRequestBase httpRequest)
        {
            var matched = false;
            if (!string.IsNullOrEmpty(QueryName))
            {
                var value = httpRequest.QueryString[QueryName];
                matched = value.EqualsOrNullEmpty(QueryValue, StringComparison.OrdinalIgnoreCase);
            }

            return matched;
        }

        public override string RuleType
        {
            get { return "QueryString"; }
            set { }
        }
        public override string DisplayText
        {
            get { return "return this.QueryName()  + '=' + this.QueryValue();"; }
            set { }
        }
        public string RuleTypeDisplayName
        {
            get { return "URL querystring"; }
        }
    }
}
