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
using System.Text.RegularExpressions;
namespace Kooboo.CMS.Sites.ABTest
{
    /// <summary>
    /// 
    /// </summary>
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IVisitRule), Kooboo.Common.ObjectContainer.Dependency.ComponentLifeStyle.Transient, Key = "IPVisitRule")]
    [System.Runtime.Serialization.DataContract(Name = "IPVisitRule")]
    [System.Runtime.Serialization.KnownType(typeof(IPVisitRule))]
    public class IPVisitRule : VisitRuleBase, IVisitRule
    {
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// The ip regular expression pattern
        /// </summary>
        [DataMember]
        public string RegexPattern { get; set; }

        public bool IsMatch(System.Web.HttpRequestBase httpRequest)
        {
            var matched = false;
            if (!string.IsNullOrEmpty(RegexPattern))
            {
                var userIP = httpRequest.UserHostAddress;
                matched = Regex.IsMatch(userIP, RegexPattern, RegexOptions.IgnoreCase);
            }
            return matched;
        }

        public override string RuleType
        {
            get { return "IP"; }
            set { }
        }

        public override string DisplayText
        {
            get { return "return this.RegexPattern();"; }
            set { }
        }


        public string RuleTypeDisplayName
        {
            get { return "User IP"; }
        }
    }
}
