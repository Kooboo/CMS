using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.CMS.ExtensionTemplate.Extension.ABTest
{
    //uncomment below code to use this as an example

    //[Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(Kooboo.CMS.Sites.ABTest.IVisitRule), Kooboo.CMS.Common.Runtime.Dependency.ComponentLifeStyle.Transient, Key = "SampleABRule")]
    //[System.Runtime.Serialization.DataContract(Name = "SampleABRule")]
    //[System.Runtime.Serialization.KnownType(typeof(SampleABRule))]
    //public class SampleABRule : Kooboo.CMS.Sites.ABTest.CustomRuleBase, Kooboo.CMS.Sites.ABTest.IVisitRule
    //{
    //    [Required(ErrorMessage = "Required")]
    //    [DataMember]
    //    public string Name { get; set; }

    //    [Required(ErrorMessage = "Required")]
    //    [Display(Name = "Header name")]
    //    [DataMember]
    //    public string HeaderName { get; set; }

    //    [Required(ErrorMessage = "Required")]
    //    [Display(Name = "Header value")]
    //    [DataMember]
    //    public string HeaderValue { get; set; }

    //    public bool IsMatch(System.Web.HttpRequestBase httpRequest)
    //    {
    //        var isMatched = false;
    //        var headerValue = httpRequest.Headers[this.Name];
    //        if (!string.IsNullOrEmpty(headerValue))
    //        {
    //            isMatched = headerValue.EqualsOrNullEmpty(HeaderValue, StringComparison.OrdinalIgnoreCase);
    //        }
    //        return isMatched;
    //    }


    //    // this name will match to the sample.cshtml. Changing this name requires changing name of the .cshtml file.
    //    public override string RuleType
    //    {
    //        get { return "Sample"; }
    //        set { }
    //    }

    //    public override string DisplayText
    //    {
    //        get { return "return this.HeaderName()  + '=' + this.HeaderValue();"; }
    //        set { }
    //    }


    //    public string RuleTypeDisplayName
    //    {
    //        get { return "Sample rule"; }
    //    }
    //}
}