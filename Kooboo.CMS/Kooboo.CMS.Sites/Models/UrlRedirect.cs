using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kooboo.Extensions;
using System.ComponentModel;
namespace Kooboo.CMS.Sites.Models
{
    public enum RedirectType
    {
        Moved_Permanently_301 = 301,
        Found_Redirect_302 = 302,
        Transfer = 200
    }
    [DataContract]
    public partial class UrlRedirect 
    {
        public UrlRedirect()
        {

        }
        public UrlRedirect(Site site)
        {
            this.Site = site;
        }
        string inputUrl = string.Empty;
        [DataMember(Order = 1)]
        public string InputUrl
        {
            get
            {
                return this.inputUrl;
            }
            set
            {
                inputUrl = value;
            }
        }
        [DataMember(Order = 4)]
        public string OutputUrl { get; set; }
        [DataMember(Order = 8)]
        public bool Regex { get; set; }
        [DataMember(Order = 12)]
        public RedirectType RedirectType { get; set; }

    }

    public partial class UrlRedirect : IPersistable
    {
        public Site Site { get; set; }

        #region override object
        public static bool operator ==(UrlRedirect obj1, UrlRedirect obj2)
        {
            if (object.Equals(obj1, obj2) == true)
            {
                return true;
            }
            if (object.Equals(obj1, null) == true || object.Equals(obj2, null) == true)
            {
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(UrlRedirect obj1, UrlRedirect obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UrlRedirect))
            {
                return false;
            }
            if (obj != null)
            {
                var urlMap = (UrlRedirect)obj;
                if (this.InputUrl.EqualsOrNullEmpty(urlMap.InputUrl, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return base.Equals(obj);
        }
        public override string ToString()
        {
            return InputUrl;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region IPersistable
        private bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            set { isDummy = value; }
        }

        public void Init(IPersistable source)
        {
            isDummy = false;
            this.Site = source.Site;
        }

        public void OnSaved()
        {
            isDummy = false;
        }

        public void OnSaving()
        {

        }

        public string DataFile
        {
            get { return new UrlRedirectsFile(this.Site).PhysicalPath; }
        }
        #endregion
    }
}
