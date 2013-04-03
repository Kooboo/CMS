using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization;

namespace Kooboo.CMS.Sites.Models
{
    [DataContractAttribute]
    public enum HttpErrorStatusCode
    {
        [EnumMember]
        Unauthorized_401 = 401,
        [EnumMember]
        NotFound_404 = 404,
        [EnumMember]
        InternalServerError_500 = 500,
        [EnumMember]
        NotImplemented_501 = 501,
        [EnumMember]
        SiteOfflined = 0,
    }

    [DataContract]
    public partial class CustomError
    {
        public Site Site { get; set; }

        [DataMember(Order = 1)]
        public HttpErrorStatusCode StatusCode { get; set; }
        [DataMember(Order = 2)]
        public string RedirectUrl { get; set; }
        [DataMember(Order = 12)]
        public RedirectType RedirectType { get; set; }
       
    }

    public partial class CustomError : IPersistable
    {
        #region Override site
        public static bool operator ==(CustomError obj1, CustomError obj2)
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
        public static bool operator !=(CustomError obj1, CustomError obj2)
        {
            return !(obj1 == obj2);
        }
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                var o = (CustomError)obj;
                if (this.StatusCode == o.StatusCode)
                {
                    return true;
                }
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return this.StatusCode.ToString();
        }
        #endregion


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
            get { return new CustomErrorsFile(Site).PhysicalPath; }
        }
    }
}
