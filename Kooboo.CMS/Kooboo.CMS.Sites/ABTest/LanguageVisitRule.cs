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
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Sites.ABTest
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IVisitRule), Kooboo.Common.ObjectContainer.Dependency.ComponentLifeStyle.Transient, Key = "LanguageVisitRule")]
    [System.Runtime.Serialization.DataContract(Name = "LanguageVisitRule")]
    [System.Runtime.Serialization.KnownType(typeof(LanguageVisitRule))]
    public class LanguageVisitRule : VisitRuleBase, IVisitRule
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string LanguageName { get; set; }

        public bool IsMatch(System.Web.HttpRequestBase httpRequest)
        {
            var matched = false;
            if (httpRequest.UserLanguages != null)
            {
                matched = httpRequest.UserLanguages.Contains(this.LanguageName, StringComparer.OrdinalIgnoreCase);
            }
            return matched;
        }
        public override string RuleType
        {
            get { return "Language"; }
            set { }
        }

        public override string DisplayText
        {
            get { return "return this.LanguageName();"; }
            set { }
        }

        public string RuleTypeDisplayName
        {
            get { return "Browser language"; }
        }
    }
}
