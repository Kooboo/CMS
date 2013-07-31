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
using System.Web;
using System.IO;
using System.Collections.Specialized;

namespace Kooboo.Web
{
    public class HttpServerUtilityBaseWrapper : HttpServerUtilityBase
    {
        // Fields
        private HttpServerUtilityBase _httpServerUtility;

        // Methods
        public HttpServerUtilityBaseWrapper(HttpServerUtilityBase httpServerUtility)
        {
            if (httpServerUtility == null)
            {
                throw new ArgumentNullException("httpServerUtility");
            }
            this._httpServerUtility = httpServerUtility;
        }

        public override void ClearError()
        {
            this._httpServerUtility.ClearError();
        }

        public override object CreateObject(string progID)
        {
            return this._httpServerUtility.CreateObject(progID);
        }

        public override object CreateObject(Type type)
        {
            return this._httpServerUtility.CreateObject(type);
        }

        public override object CreateObjectFromClsid(string clsid)
        {
            return this._httpServerUtility.CreateObjectFromClsid(clsid);
        }

        public override void Execute(string path)
        {
            this._httpServerUtility.Execute(path);
        }

        public override void Execute(string path, bool preserveForm)
        {
            this._httpServerUtility.Execute(path, preserveForm);
        }

        public override void Execute(string path, TextWriter writer)
        {
            this._httpServerUtility.Execute(path, writer);
        }

        public override void Execute(string path, TextWriter writer, bool preserveForm)
        {
            this._httpServerUtility.Execute(path, writer, preserveForm);
        }

        public override void Execute(IHttpHandler handler, TextWriter writer, bool preserveForm)
        {
            this._httpServerUtility.Execute(handler, writer, preserveForm);
        }

        public override Exception GetLastError()
        {
            return this._httpServerUtility.GetLastError();
        }

        public override string HtmlDecode(string s)
        {
            return this._httpServerUtility.HtmlDecode(s);
        }

        public override void HtmlDecode(string s, TextWriter output)
        {
            this._httpServerUtility.HtmlDecode(s, output);
        }

        public override string HtmlEncode(string s)
        {
            return this._httpServerUtility.HtmlEncode(s);
        }

        public override void HtmlEncode(string s, TextWriter output)
        {
            this._httpServerUtility.HtmlEncode(s, output);
        }

        public override string MapPath(string path)
        {
            return this._httpServerUtility.MapPath(path);
        }

        public override void Transfer(string path)
        {
            this._httpServerUtility.Transfer(path);
        }

        public override void Transfer(string path, bool preserveForm)
        {
            this._httpServerUtility.Transfer(path, preserveForm);
        }

        public override void Transfer(IHttpHandler handler, bool preserveForm)
        {
            this._httpServerUtility.Transfer(handler, preserveForm);
        }

        public override void TransferRequest(string path)
        {
            this._httpServerUtility.TransferRequest(path);
        }

        public override void TransferRequest(string path, bool preserveForm)
        {
            this._httpServerUtility.TransferRequest(path, preserveForm);
        }

        public override void TransferRequest(string path, bool preserveForm, string method, NameValueCollection headers)
        {
            this._httpServerUtility.TransferRequest(path, preserveForm, method, headers);
        }

        public override string UrlDecode(string s)
        {
            return this._httpServerUtility.UrlDecode(s);
        }

        public override void UrlDecode(string s, TextWriter output)
        {
            this._httpServerUtility.UrlDecode(s, output);
        }

        public override string UrlEncode(string s)
        {
            return this._httpServerUtility.UrlEncode(s);
        }

        public override void UrlEncode(string s, TextWriter output)
        {
            this._httpServerUtility.UrlEncode(s, output);
        }

        public override string UrlPathEncode(string s)
        {
            return this._httpServerUtility.UrlPathEncode(s);
        }

        public override byte[] UrlTokenDecode(string input)
        {
            return HttpServerUtility.UrlTokenDecode(input);
        }

        public override string UrlTokenEncode(byte[] input)
        {
            return HttpServerUtility.UrlTokenEncode(input);
        }

        // Properties
        public override string MachineName
        {
            get
            {
                return this._httpServerUtility.MachineName;
            }
        }

        public override int ScriptTimeout
        {
            get
            {
                return this._httpServerUtility.ScriptTimeout;
            }
            set
            {
                this._httpServerUtility.ScriptTimeout = value;
            }
        }
    }

}
