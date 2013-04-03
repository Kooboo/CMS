using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.IO;
using System.Collections.Specialized;

namespace Kooboo.Web
{
    public class HttpResponseBaseWrapper : HttpResponseBase
    {
        // Fields
        private HttpResponseBase _httpResponse;

        // Methods
        public HttpResponseBaseWrapper(HttpResponseBase httpResponse)
        {
            if (httpResponse == null)
            {
                throw new ArgumentNullException("httpResponse");
            }
            this._httpResponse = httpResponse;
        }

        public override void AddCacheDependency(params CacheDependency[] dependencies)
        {
            this._httpResponse.AddCacheDependency(dependencies);
        }

        public override void AddCacheItemDependencies(ArrayList cacheKeys)
        {
            this._httpResponse.AddCacheItemDependencies(cacheKeys);
        }

        public override void AddCacheItemDependencies(string[] cacheKeys)
        {
            this._httpResponse.AddCacheItemDependencies(cacheKeys);
        }

        public override void AddCacheItemDependency(string cacheKey)
        {
            this._httpResponse.AddCacheItemDependency(cacheKey);
        }

        public override void AddFileDependencies(string[] filenames)
        {
            this._httpResponse.AddFileDependencies(filenames);
        }

        public override void AddFileDependencies(ArrayList filenames)
        {
            this._httpResponse.AddFileDependencies(filenames);
        }

        public override void AddFileDependency(string filename)
        {
            this._httpResponse.AddFileDependency(filename);
        }

        public override void AddHeader(string name, string value)
        {
            this._httpResponse.AddHeader(name, value);
        }

        public override void AppendCookie(HttpCookie cookie)
        {
            this._httpResponse.AppendCookie(cookie);
        }

        public override void AppendHeader(string name, string value)
        {
            this._httpResponse.AppendHeader(name, value);
        }

        public override void AppendToLog(string param)
        {
            this._httpResponse.AppendToLog(param);
        }

        public override string ApplyAppPathModifier(string virtualPath)
        {
            return this._httpResponse.ApplyAppPathModifier(virtualPath);
        }

        public override void BinaryWrite(byte[] buffer)
        {
            this._httpResponse.BinaryWrite(buffer);
        }

        public override void Clear()
        {
            this._httpResponse.Clear();
        }

        public override void ClearContent()
        {
            this._httpResponse.ClearContent();
        }

        public override void ClearHeaders()
        {
            this._httpResponse.ClearHeaders();
        }

        public override void Close()
        {
            this._httpResponse.Close();
        }

        public override void DisableKernelCache()
        {
            this._httpResponse.DisableKernelCache();
        }

        public override void End()
        {
            this._httpResponse.End();
        }

        public override void Flush()
        {
            this._httpResponse.Flush();
        }

        public override void Pics(string value)
        {
            this._httpResponse.Pics(value);
        }

        public override void Redirect(string url)
        {
            this._httpResponse.Redirect(url);
        }

        public override void Redirect(string url, bool endResponse)
        {
            this._httpResponse.Redirect(url, endResponse);
        }

        public override void RemoveOutputCacheItem(string path)
        {
            HttpResponse.RemoveOutputCacheItem(path);
        }

        public override void SetCookie(HttpCookie cookie)
        {
            this._httpResponse.SetCookie(cookie);
        }

        public override void TransmitFile(string filename)
        {
            this._httpResponse.TransmitFile(filename);
        }

        public override void TransmitFile(string filename, long offset, long length)
        {
            this._httpResponse.TransmitFile(filename, offset, length);
        }

        public override void Write(char ch)
        {
            this._httpResponse.Write(ch);
        }

        public override void Write(object obj)
        {
            this._httpResponse.Write(obj);
        }

        public override void Write(string s)
        {
            this._httpResponse.Write(s);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            this._httpResponse.Write(buffer, index, count);
        }

        public override void WriteFile(string filename)
        {
            this._httpResponse.WriteFile(filename);
        }

        public override void WriteFile(string filename, bool readIntoMemory)
        {
            this._httpResponse.WriteFile(filename, readIntoMemory);
        }

        public override void WriteFile(IntPtr fileHandle, long offset, long size)
        {
            this._httpResponse.WriteFile(fileHandle, offset, size);
        }

        public override void WriteFile(string filename, long offset, long size)
        {
            this._httpResponse.WriteFile(filename, offset, size);
        }

        public override void WriteSubstitution(HttpResponseSubstitutionCallback callback)
        {
            this._httpResponse.WriteSubstitution(callback);
        }

        // Properties
        public override bool Buffer
        {
            get
            {
                return this._httpResponse.Buffer;
            }
            set
            {
                this._httpResponse.Buffer = value;
            }
        }

        public override bool BufferOutput
        {
            get
            {
                return this._httpResponse.BufferOutput;
            }
            set
            {
                this._httpResponse.BufferOutput = value;
            }
        }

        public override HttpCachePolicyBase Cache
        {
            get
            {
                return new HttpCachePolicyBaseWrapper(this._httpResponse.Cache);
            }
        }

        public override string CacheControl
        {
            get
            {
                return this._httpResponse.CacheControl;
            }
            set
            {
                this._httpResponse.CacheControl = value;
            }
        }

        public override string Charset
        {
            get
            {
                return this._httpResponse.Charset;
            }
            set
            {
                this._httpResponse.Charset = value;
            }
        }

        public override Encoding ContentEncoding
        {
            get
            {
                return this._httpResponse.ContentEncoding;
            }
            set
            {
                this._httpResponse.ContentEncoding = value;
            }
        }

        public override string ContentType
        {
            get
            {
                return this._httpResponse.ContentType;
            }
            set
            {
                this._httpResponse.ContentType = value;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return this._httpResponse.Cookies;
            }
        }

        public override int Expires
        {
            get
            {
                return this._httpResponse.Expires;
            }
            set
            {
                this._httpResponse.Expires = value;
            }
        }

        public override DateTime ExpiresAbsolute
        {
            get
            {
                return this._httpResponse.ExpiresAbsolute;
            }
            set
            {
                this._httpResponse.ExpiresAbsolute = value;
            }
        }

        public override Stream Filter
        {
            get
            {
                return this._httpResponse.Filter;
            }
            set
            {
                this._httpResponse.Filter = value;
            }
        }

        public override Encoding HeaderEncoding
        {
            get
            {
                return this._httpResponse.HeaderEncoding;
            }
            set
            {
                this._httpResponse.HeaderEncoding = value;
            }
        }

        public override NameValueCollection Headers
        {
            get
            {
                return this._httpResponse.Headers;
            }
        }

        public override bool IsClientConnected
        {
            get
            {
                return this._httpResponse.IsClientConnected;
            }
        }

        public override bool IsRequestBeingRedirected
        {
            get
            {
                return this._httpResponse.IsRequestBeingRedirected;
            }
        }

        public override TextWriter Output
        {
            get
            {
                return this._httpResponse.Output;
            }
        }

        public override Stream OutputStream
        {
            get
            {
                return this._httpResponse.OutputStream;
            }
        }

        public override string RedirectLocation
        {
            get
            {
                return this._httpResponse.RedirectLocation;
            }
            set
            {
                this._httpResponse.RedirectLocation = value;
            }
        }

        public override string Status
        {
            get
            {
                return this._httpResponse.Status;
            }
            set
            {
                this._httpResponse.Status = value;
            }
        }

        public override int StatusCode
        {
            get
            {
                return this._httpResponse.StatusCode;
            }
            set
            {
                this._httpResponse.StatusCode = value;
            }
        }

        public override string StatusDescription
        {
            get
            {
                return this._httpResponse.StatusDescription;
            }
            set
            {
                this._httpResponse.StatusDescription = value;
            }
        }

        public override int SubStatusCode
        {
            get
            {
                return this._httpResponse.SubStatusCode;
            }
            set
            {
                this._httpResponse.SubStatusCode = value;
            }
        }

        public override bool SuppressContent
        {
            get
            {
                return this._httpResponse.SuppressContent;
            }
            set
            {
                this._httpResponse.SuppressContent = value;
            }
        }

        public override bool TrySkipIisCustomErrors
        {
            get
            {
                return this._httpResponse.TrySkipIisCustomErrors;
            }
            set
            {
                this._httpResponse.TrySkipIisCustomErrors = value;
            }
        }

    }
}
