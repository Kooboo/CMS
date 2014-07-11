
using Kooboo.Common.Misc;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.CMS.Sites.Contact {
    public class ContactSiteModel {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.EmailAddress, ErrorMessage = "Invalid email address")]
        public virtual string From { get; set; }

        [Required(ErrorMessage = "Required")]
        public virtual string Subject { get; set; }

        [Required(ErrorMessage = "Required")]
        public virtual string Body { get; set; }

        public virtual string EmailBody { get; set; }
    }
}
