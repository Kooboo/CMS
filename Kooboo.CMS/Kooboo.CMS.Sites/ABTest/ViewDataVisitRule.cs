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
namespace Kooboo.CMS.Sites.VisitRule
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IVisitRule), Kooboo.CMS.Common.Runtime.Dependency.ComponentLifeStyle.Transient, Key = "ViewDataVisitRule")]
    [System.Runtime.Serialization.DataContract(Name = "ViewDataVisitRule")]
    [System.Runtime.Serialization.KnownType(typeof(ViewDataVisitRule))]
    public class ViewDataVisitRule : VisitRuleBase, IVisitRule
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FieldName { get; set; }
        [DataMember]
        public string Value { get; set; }

        public bool IsMatch(ControllerContext controllerContext)
        {
            throw new NotImplementedException();
        }

        public override string RuleType
        {
            get { return "ViewData"; }
            set { }
        }
    }
}
