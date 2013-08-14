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
using Kooboo.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
namespace Kooboo.CMS.Sites.ABTest
{

    /// <summary>
    /// 
    /// </summary>
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IVisitRule), Kooboo.CMS.Common.Runtime.Dependency.ComponentLifeStyle.Transient, Key = "UserAgentVisitRule")]
    [System.Runtime.Serialization.DataContract(Name = "UserAgentVisitRule")]
    [System.Runtime.Serialization.KnownType(typeof(UserAgentVisitRule))]
    public class UserAgentVisitRule : VisitRuleBase, IVisitRule
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string RegexPattern { get; set; }

        public bool IsMatch(System.Web.HttpRequestBase httpRequest)
        {
            var matched = false;
            if (!string.IsNullOrEmpty(RegexPattern))
            {
                var userAgent = httpRequest.UserAgent;
                if (!string.IsNullOrEmpty(userAgent))
                {
                    matched = Regex.IsMatch(userAgent, RegexPattern, RegexOptions.IgnoreCase);
                }
            }
            return matched;
        }

        public override string RuleType
        {
            get { return "UserAgent"; }
            set { }
        }

        public override string DisplayText
        {
            get { return "return this.RegexPattern();"; }
            set { }
        }
        public string RuleTypeDisplayName
        {
            get { return "Browser useragent"; }
        }
    }
}
