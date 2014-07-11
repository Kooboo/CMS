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
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IVisitRule), Kooboo.Common.ObjectContainer.Dependency.ComponentLifeStyle.Transient, Key = "RandomVisitRule")]
    [System.Runtime.Serialization.DataContract(Name = "RandomVisitRule")]
    [System.Runtime.Serialization.KnownType(typeof(RandomVisitRule))]
    public class RandomVisitRule : VisitRuleBase, IVisitRule
    {
        static Random random = new Random();
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the percent.
        /// The value 10 means 10%
        /// </summary>
        /// <value>
        /// The percent.
        /// </value>
        [DataMember]
        public int Percent { get; set; }

        public bool IsMatch(System.Web.HttpRequestBase httpRequest)
        {
            var matched = false;
            if (Percent > 0)
            {
                var value = random.NextDouble();
                if (value < Percent / 100.0)
                {
                    matched = true;
                }
            }
            return matched;
        }

        public override string RuleType
        {
            get { return "Random"; }
            set { }
        }

        public override string DisplayText
        {
            get { return "return this.Percent() + ' %';"; }
            set { }
        }
        public string RuleTypeDisplayName
        {
            get { return "Percentage random"; }
        }
    }
}
