using Kooboo.CMS.Common;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.ABTest
{
    [System.Runtime.Serialization.DataContract]
    public abstract class CustomRuleBase
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
                var baseDir = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IBaseDir>();
                return UrlUtility.Combine(baseDir.Cms_DataVirtualPath, "Views", "ABRuleTemplates", RuleType + ".cshtml");
            }
            set { }
        }
    }
}
