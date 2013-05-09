using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.ABTest
{
    [DataContract]
    public abstract class VisitRuleBase
    {
        /// <summary>
        /// The DataMember on this field is only used for the JSON.NET serializer.
        /// </summary>
        [DataMember]
        public abstract string RuleType { get; set; }
        /// <summary>
        /// The DataMember on this field is only used for the JSON.NET serializer.
        /// </summary>
        [DataMember]
        public abstract string DisplayText { get; set; }
        /// <summary>
        /// The DataMember on this field is only used for the JSON.NET serializer.
        /// </summary>
        [DataMember]
        public virtual string TemplateVirtualPath
        {
            get
            {
                return string.Format("~/Areas/Sites/Views/ABRuleSetting/RuleTemplates/{0}.cshtml", RuleType);
            }
            set
            {
            }
        }
    }
}
