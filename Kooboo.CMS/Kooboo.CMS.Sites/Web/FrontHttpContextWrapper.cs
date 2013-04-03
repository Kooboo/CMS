using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Sites.Web
{
    public class FrontHttpContextWrapper : System.Web.HttpContextWrapper
    {
        private readonly HttpContext _context;

        public FrontHttpContextWrapper(HttpContext httpContext)
            : base(httpContext)
        {
            _context = httpContext;
        }
        HttpRequestBase request;
        public override HttpRequestBase Request
        {
            get
            {
                if (request == null)
                {
                    request = new FrontHttpRequestWrapper(_context.Request);
                }
                return request;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return new FrontHttpResponseWrapper(_context.Response, this);
            }
        }

        public FrontHttpRequestWrapper RequestWrapper
        {
            get
            {
                return (FrontHttpRequestWrapper)this.Request;
            }
        }
        public FrontHttpResponseWrapper ResponseWrapper
        {
            get
            {
                return (FrontHttpResponseWrapper)this.Response;
            }
        }
    }
}
