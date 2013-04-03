using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Security.Principal;
using System.IO;
using System.Web;

namespace Kooboo.Web
{
    public class HttpRequestBaseWrapper : HttpRequestBase
    {
        // Fields
        private HttpRequestBase _httpRequest;

        // Methods
        public HttpRequestBaseWrapper(HttpRequestBase httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException("httpRequest");
            }
            this._httpRequest = httpRequest;
        }

        public override byte[] BinaryRead(int count)
        {
            return this._httpRequest.BinaryRead(count);
        }

        public override int[] MapImageCoordinates(string imageFieldName)
        {
            return this._httpRequest.MapImageCoordinates(imageFieldName);
        }

        public override string MapPath(string virtualPath)
        {
            return this._httpRequest.MapPath(virtualPath);
        }

        public override string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping)
        {
            return this._httpRequest.MapPath(virtualPath, baseVirtualDir, allowCrossAppMapping);
        }

        public override void SaveAs(string filename, bool includeHeaders)
        {
            this._httpRequest.SaveAs(filename, includeHeaders);
        }

        public override void ValidateInput()
        {
            this._httpRequest.ValidateInput();
        }

        // Properties
        public override string[] AcceptTypes
        {
            get
            {
                return this._httpRequest.AcceptTypes;
            }
        }

        public override string AnonymousID
        {
            get
            {
                return this._httpRequest.AnonymousID;
            }
        }

        public override string ApplicationPath
        {
            get
            {
                return this._httpRequest.ApplicationPath;
            }
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                return this._httpRequest.AppRelativeCurrentExecutionFilePath;
            }
        }

        public override HttpBrowserCapabilitiesBase Browser
        {
            get
            {
                return new HttpBrowserCapabilitiesBaseWrapper(this._httpRequest.Browser);
            }
        }

        public override HttpClientCertificate ClientCertificate
        {
            get
            {
                return this._httpRequest.ClientCertificate;
            }
        }

        public override Encoding ContentEncoding
        {
            get
            {
                return this._httpRequest.ContentEncoding;
            }
            set
            {
                this._httpRequest.ContentEncoding = value;
            }
        }

        public override int ContentLength
        {
            get
            {
                return this._httpRequest.ContentLength;
            }
        }

        public override string ContentType
        {
            get
            {
                return this._httpRequest.ContentType;
            }
            set
            {
                this._httpRequest.ContentType = value;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return this._httpRequest.Cookies;
            }
        }

        public override string CurrentExecutionFilePath
        {
            get
            {
                return this._httpRequest.CurrentExecutionFilePath;
            }
        }

        public override string FilePath
        {
            get
            {
                return this._httpRequest.FilePath;
            }
        }

        public override HttpFileCollectionBase Files
        {
            get
            {
                return new HttpFileCollectionBaseWrapper(this._httpRequest.Files);
            }
        }

        public override Stream Filter
        {
            get
            {
                return this._httpRequest.Filter;
            }
            set
            {
                this._httpRequest.Filter = value;
            }
        }

        public override NameValueCollection Form
        {
            get
            {
                return this._httpRequest.Form;
            }
        }

        public override NameValueCollection Headers
        {
            get
            {
                return this._httpRequest.Headers;
            }
        }

        public override string HttpMethod
        {
            get
            {
                return this._httpRequest.HttpMethod;
            }
        }

        public override Stream InputStream
        {
            get
            {
                return this._httpRequest.InputStream;
            }
        }

        public override bool IsAuthenticated
        {
            get
            {
                return this._httpRequest.IsAuthenticated;
            }
        }

        public override bool IsLocal
        {
            get
            {
                return this._httpRequest.IsLocal;
            }
        }

        public override bool IsSecureConnection
        {
            get
            {
                return this._httpRequest.IsSecureConnection;
            }
        }

        public override string this[string key]
        {
            get
            {
                return this._httpRequest[key];
            }
        }

        public override WindowsIdentity LogonUserIdentity
        {
            get
            {
                return this._httpRequest.LogonUserIdentity;
            }
        }

        public override NameValueCollection Params
        {
            get
            {
                return this._httpRequest.Params;
            }
        }

        public override string Path
        {
            get
            {
                return this._httpRequest.Path;
            }
        }

        public override string PathInfo
        {
            get
            {
                return this._httpRequest.PathInfo;
            }
        }

        public override string PhysicalApplicationPath
        {
            get
            {
                return this._httpRequest.PhysicalApplicationPath;
            }
        }

        public override string PhysicalPath
        {
            get
            {
                return this._httpRequest.PhysicalPath;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return this._httpRequest.QueryString;
            }
        }

        public override string RawUrl
        {
            get
            {
                return this._httpRequest.RawUrl;
            }
        }

        public override string RequestType
        {
            get
            {
                return this._httpRequest.RequestType;
            }
            set
            {
                this._httpRequest.RequestType = value;
            }
        }

        public override NameValueCollection ServerVariables
        {
            get
            {
                return this._httpRequest.ServerVariables;
            }
        }

        public override int TotalBytes
        {
            get
            {
                return this._httpRequest.TotalBytes;
            }
        }

        public override Uri Url
        {
            get
            {
                return this._httpRequest.Url;
            }
        }

        public override Uri UrlReferrer
        {
            get
            {
                return this._httpRequest.UrlReferrer;
            }
        }

        public override string UserAgent
        {
            get
            {
                return this._httpRequest.UserAgent;
            }
        }

        public override string UserHostAddress
        {
            get
            {
                return this._httpRequest.UserHostAddress;
            }
        }

        public override string UserHostName
        {
            get
            {
                return this._httpRequest.UserHostName;
            }
        }

        public override string[] UserLanguages
        {
            get
            {
                return this._httpRequest.UserLanguages;
            }
        }
    }

}
