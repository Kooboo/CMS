using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Globalization;
using System.Web.Caching;
using System.Web.Profile;
using System.Collections;
using System.Security.Principal;

namespace Kooboo.Web
{
    public class HttpContextBaseWrapper : HttpContextBase
    {
        // Fields
        private readonly HttpContextBase _context;

        // Methods
        public HttpContextBaseWrapper(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            this._context = httpContext;
        }

        public override void AddError(Exception errorInfo)
        {
            this._context.AddError(errorInfo);
        }

        public override void ClearError()
        {
            this._context.ClearError();
        }

        public override object GetGlobalResourceObject(string classKey, string resourceKey)
        {
            return HttpContext.GetGlobalResourceObject(classKey, resourceKey);
        }

        public override object GetGlobalResourceObject(string classKey, string resourceKey, CultureInfo culture)
        {
            return HttpContext.GetGlobalResourceObject(classKey, resourceKey, culture);
        }

        public override object GetLocalResourceObject(string virtualPath, string resourceKey)
        {
            return HttpContext.GetLocalResourceObject(virtualPath, resourceKey);
        }

        public override object GetLocalResourceObject(string virtualPath, string resourceKey, CultureInfo culture)
        {
            return HttpContext.GetLocalResourceObject(virtualPath, resourceKey, culture);
        }

        public override object GetSection(string sectionName)
        {
            return this._context.GetSection(sectionName);
        }

        public override object GetService(Type serviceType)
        {
            return ((IServiceProvider)this._context).GetService(serviceType);
        }

        public override void RewritePath(string path)
        {
            this._context.RewritePath(path);
        }

        public override void RewritePath(string path, bool rebaseClientPath)
        {
            this._context.RewritePath(path, rebaseClientPath);
        }

        public override void RewritePath(string filePath, string pathInfo, string queryString)
        {
            this._context.RewritePath(filePath, pathInfo, queryString);
        }

        public override void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath)
        {
            this._context.RewritePath(filePath, pathInfo, queryString, setClientFilePath);
        }

        // Properties
        public override Exception[] AllErrors
        {
            get
            {
                return this._context.AllErrors;
            }
        }

        public override HttpApplicationStateBase Application
        {
            get
            {
                return new HttpApplicationStateBaseWrapper(this._context.Application);
            }
        }

        public override HttpApplication ApplicationInstance
        {
            get
            {
                return this._context.ApplicationInstance;
            }
            set
            {
                this._context.ApplicationInstance = value;
            }
        }

        public override Cache Cache
        {
            get
            {
                return this._context.Cache;
            }
        }

        public override IHttpHandler CurrentHandler
        {
            get
            {
                return this._context.CurrentHandler;
            }
        }

        public override RequestNotification CurrentNotification
        {
            get
            {
                return this._context.CurrentNotification;
            }
        }

        public override Exception Error
        {
            get
            {
                return this._context.Error;
            }
        }

        public override IHttpHandler Handler
        {
            get
            {
                return this._context.Handler;
            }
            set
            {
                this._context.Handler = value;
            }
        }

        public override bool IsCustomErrorEnabled
        {
            get
            {
                return this._context.IsCustomErrorEnabled;
            }
        }

        public override bool IsDebuggingEnabled
        {
            get
            {
                return this._context.IsDebuggingEnabled;
            }
        }

        public override bool IsPostNotification
        {
            get
            {
                return this._context.IsDebuggingEnabled;
            }
        }

        public override IDictionary Items
        {
            get
            {
                return this._context.Items;
            }
        }

        public override IHttpHandler PreviousHandler
        {
            get
            {
                return this._context.PreviousHandler;
            }
        }

        public override ProfileBase Profile
        {
            get
            {
                return this._context.Profile;
            }
        }

        public override HttpRequestBase Request
        {
            get
            {
                return new HttpRequestBaseWrapper(this._context.Request);
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return new HttpResponseBaseWrapper(this._context.Response);
            }
        }

        public override HttpServerUtilityBase Server
        {
            get
            {
                return new HttpServerUtilityBaseWrapper(this._context.Server);
            }
        }

        public override HttpSessionStateBase Session
        {
            get
            {
                return _context.Session;
            }
        }

        public override bool SkipAuthorization
        {
            get
            {
                return this._context.SkipAuthorization;
            }
            set
            {
                this._context.SkipAuthorization = value;
            }
        }

        public override DateTime Timestamp
        {
            get
            {
                return this._context.Timestamp;
            }
        }

        public override TraceContext Trace
        {
            get
            {
                return this._context.Trace;
            }
        }

        public override IPrincipal User
        {
            get
            {
                return this._context.User;
            }
            set
            {
                this._context.User = value;
            }
        }
    }

}
