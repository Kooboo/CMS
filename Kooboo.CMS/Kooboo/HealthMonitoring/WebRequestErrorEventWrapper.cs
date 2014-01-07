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
using System.Web;
using System.Web.Management;
using System.Globalization;
using System.Text;
using System.Security.Principal;
using System.Diagnostics;
using System.Threading;

namespace Kooboo.HealthMonitoring
{
    #region WebEventFieldType
    /// <summary>
    /// 
    /// </summary>
    internal enum WebEventFieldType
    {
        String,
        Int,
        Bool,
        Long,
        Date
    }
    #endregion

    #region WebEventFieldData
    internal class WebEventFieldData
    {
        // Fields
        private string _data;
        private string _name;
        private WebEventFieldType _type;

        // Methods
        public WebEventFieldData(string name, string data, WebEventFieldType type)
        {
            this._name = name;
            this._data = data;
            this._type = type;
        }

        // Properties
        public string Data { get { return _data; } }
        public string Name { get { return _name; } }
        public WebEventFieldType Type { get { return _type; } }
    }
    #endregion

    #region WebEventFormatter
    /// <summary>
    /// 
    /// </summary>
    public class WebEventFormatter
    {
        #region Fields
        // Fields
        private int _level = 0;
        private StringBuilder _sb = new StringBuilder();
        private int _tabSize = 4;
        #endregion

        #region .ctor

        // Methods
        internal WebEventFormatter()
        {
        }

        #endregion

        #region Methods

        private void AddTab()
        {
            for (int i = this._level; i > 0; i--)
            {
                this._sb.Append(' ', this._tabSize);
            }
        }

        public void AppendLine(string s)
        {
            this.AddTab();
            this._sb.Append(s);
            this._sb.Append('\n');
        }

        public override string ToString()
        {
            return this._sb.ToString();
        }
        #endregion

        #region Properties
        // Properties
        public int IndentationLevel
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = Math.Max(value, 0);
            }
        }

        public int TabSize
        {
            get
            {
                return this._tabSize;
            }
            set
            {
                this._tabSize = Math.Max(value, 0);
            }
        }
        #endregion
    }

    #endregion

    #region WebRequestInformation
    public sealed class WebRequestInformation
    {
        #region Fields
        // Fields
        private string _accountName;
        private IPrincipal _iprincipal;
        private string _requestPath;
        private string _requestUrl;
        private string _userHostAddress;
        #endregion

        #region Methods
        // Methods
        public WebRequestInformation()
        {
            //InternalSecurityPermissions.ControlPrincipal.Assert();
            HttpContext current = HttpContext.Current;
            HttpRequest request = null;
            if (current != null)
            {
                //bool hideRequestResponse = current.HideRequestResponse;
                //current.HideRequestResponse = false;
                request = current.Request;
                //current.HideRequestResponse = hideRequestResponse;
                this._iprincipal = current.User;
            }
            else
            {
                this._iprincipal = null;
            }
            if (request == null)
            {
                this._requestUrl = string.Empty;
                this._requestPath = string.Empty;
                this._userHostAddress = string.Empty;
            }
            else
            {
                this._requestUrl = request.RawUrl;
                this._requestPath = request.Path;
                this._userHostAddress = request.UserHostAddress;
            }
            this._accountName = WindowsIdentity.GetCurrent().Name;
        }

        public void FormatToString(WebEventFormatter formatter)
        {
            string name;
            string authenticationType;
            bool isAuthenticated;
            if (this.Principal == null)
            {
                name = string.Empty;
                authenticationType = string.Empty;
                isAuthenticated = false;
            }
            else
            {
                IIdentity identity = this.Principal.Identity;
                name = identity.Name;
                isAuthenticated = identity.IsAuthenticated;
                authenticationType = identity.AuthenticationType;
            }
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_request_url", this.RequestUrl));
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_request_path", this.RequestPath));
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_user_host_address", this.UserHostAddress));
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_user", name));
            if (isAuthenticated)
            {
                formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_is_authenticated"));
            }
            else
            {
                formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_is_not_authenticated"));
            }
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_authentication_type", authenticationType));
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_thread_account_name", this.ThreadAccountName));
        }

        #endregion

        #region Properties

        // Properties
        public IPrincipal Principal
        {
            get
            {
                return this._iprincipal;
            }
        }

        public string RequestPath
        {
            get
            {
                return this._requestPath;
            }
        }

        public string RequestUrl
        {
            get
            {
                return this._requestUrl;
            }
        }

        public string ThreadAccountName
        {
            get
            {
                return this._accountName;
            }
        }

        public string UserHostAddress
        {
            get
            {
                return this._userHostAddress;
            }
        }
        #endregion
    }
    #endregion


    #region WebThreadInformation
    public sealed class WebThreadInformation
    {
        // Fields
        private string _accountName;
        private bool _isImpersonating;
        private string _stackTrace;
        private int _threadId;
        internal const string IsImpersonatingKey = "ASPIMPERSONATING";

        // Methods
        public WebThreadInformation(Exception exception)
        {
            this._threadId = Thread.CurrentThread.ManagedThreadId;
            this._accountName = WindowsIdentity.GetCurrent().Name;
            if (exception != null)
            {
                this._stackTrace = new StackTrace(exception, true).ToString();
            }
            else
            {
                this._stackTrace = string.Empty;
            }
            this._isImpersonating = exception.Data.Contains("ASPIMPERSONATING");

        }
        public void FormatToString(WebEventFormatter formatter)
        {
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_thread_id", this.ThreadID.ToString(CultureInfo.InstalledUICulture)));
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_thread_account_name", this.ThreadAccountName));
            if (this.IsImpersonating)
            {
                formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_is_impersonating"));
            }
            else
            {
                formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_is_not_impersonating"));
            }
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_stack_trace", this.StackTrace));
        }

        // Properties
        public bool IsImpersonating { get { return _isImpersonating; } }
        public string StackTrace { get { return _stackTrace; } }
        public string ThreadAccountName { get { return _accountName; } }
        public int ThreadID { get { return _threadId; } }
    }

    #endregion

    #region WebEventFormatterExtensions
    internal static class WebEventFormatterExtensions
    {
        public static void FormatToString(this WebApplicationInformation info, WebEventFormatter formatter)
        {
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_application_domain", info.ApplicationDomain));
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_trust_level", info.TrustLevel));
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_application_virtual_path", info.ApplicationVirtualPath));
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_application_path", info.ApplicationPath));
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_machine_name", info.MachineName));
        }

    }
    #endregion

    #region WebBaseEventWrapper
    /// <summary>
    /// Wrapper WebBaseEvent to expose some api.
    /// </summary>
    public class WebBaseEventWrapper : WebBaseEvent
    {
        //public WebRequestErrorEventWrapper(Exception e)
        //    : this("", null, WebEventCodes.RuntimeErrorUnhandledException, e)
        //{

        //}
        public WebBaseEventWrapper(string msg,
            object eventSource, int eventCode) :
            base(msg, eventSource, eventCode)
        {

        }

        #region FormatResourceStringWithCache

        public static string FormatResourceStringWithCache(string key)
        {
            var cacheInternal = HttpRuntime.Cache;
            string str2 = CreateWebEventResourceCacheKey(key);
            string str = (string)cacheInternal.Get(str2);
            if (str == null)
            {
                str = SR.GetString(key);
                if (str != null)
                {
                    cacheInternal.Insert(str2, str);
                }
            }
            return str;
        }

        private static string CreateWebEventResourceCacheKey(string key)
        {
            return ("x" + key);
        }
        public static string FormatResourceStringWithCache(string key, string arg0)
        {
            string format = FormatResourceStringWithCache(key);
            if (format == null)
            {
                return null;
            }
            return string.Format(format, arg0);
        }

        #endregion

        internal virtual void GenerateFieldsForMarshal(List<WebEventFieldData> fields)
        {
            fields.Add(new WebEventFieldData("EventTime", this.EventTimeUtc.ToString(), WebEventFieldType.String));
            fields.Add(new WebEventFieldData("EventID", this.EventID.ToString(), WebEventFieldType.String));
            fields.Add(new WebEventFieldData("EventMessage", this.Message, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("ApplicationDomain", ApplicationInformation.ApplicationDomain, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("TrustLevel", ApplicationInformation.TrustLevel, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("ApplicationVirtualPath", ApplicationInformation.ApplicationVirtualPath, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("ApplicationPath", ApplicationInformation.ApplicationPath, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("MachineName", ApplicationInformation.MachineName, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("EventCode", this.EventCode.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Int));
            fields.Add(new WebEventFieldData("EventDetailCode", this.EventDetailCode.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Int));
            fields.Add(new WebEventFieldData("SequenceNumber", this.EventSequence.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Long));
#if MONO
			
#else
            //fields.Add(new WebEventFieldData("Occurrence", this.EventOccurrence.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Long));
#endif
        }

        protected virtual void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
        {
            formatter.AppendLine(FormatResourceStringWithCache("Webevent_event_code", this.EventCode.ToString(CultureInfo.InstalledUICulture)));
            formatter.AppendLine(FormatResourceStringWithCache("Webevent_event_message", this.Message));
            formatter.AppendLine(FormatResourceStringWithCache("Webevent_event_time", this.EventTime.ToString(CultureInfo.InstalledUICulture)));
            formatter.AppendLine(FormatResourceStringWithCache("Webevent_event_time_Utc", this.EventTimeUtc.ToString(CultureInfo.InstalledUICulture)));
            formatter.AppendLine(FormatResourceStringWithCache("Webevent_event_id", this.EventID.ToString("N", CultureInfo.InstalledUICulture)));
            formatter.AppendLine(FormatResourceStringWithCache("Webevent_event_sequence", this.EventSequence.ToString(CultureInfo.InstalledUICulture)));
#if MONO
				
#else
            //formatter.AppendLine(FormatResourceStringWithCache("Webevent_event_occurrence", this.EventOccurrence.ToString(CultureInfo.InstalledUICulture)));
#endif
            formatter.AppendLine(FormatResourceStringWithCache("Webevent_event_detail_code", this.EventDetailCode.ToString(CultureInfo.InstalledUICulture)));
            if (includeAppInfo)
            {
                formatter.AppendLine(string.Empty);
                formatter.AppendLine(FormatResourceStringWithCache("Webevent_event_application_information"));
                formatter.IndentationLevel++;
                ApplicationInformation.FormatToString(formatter);
                formatter.IndentationLevel--;
            }
        }

        internal bool IsSystemEvent
        {
            get
            {
                return (this.EventCode < 0x186a0);
            }
        }

        public override string ToString()
        {
            return this.ToString(true, true);
        }
        public override string ToString(bool includeAppInfo, bool includeCustomEventDetails)
        {
            WebEventFormatter formatter = new WebEventFormatter();
            this.FormatToString(formatter, includeAppInfo);
            if (!this.IsSystemEvent && includeCustomEventDetails)
            {
                formatter.AppendLine(string.Empty);
                formatter.AppendLine(SR.GetString("Webevent_event_custom_event_details"));
                formatter.IndentationLevel++;
                this.FormatCustomEventDetails(formatter);
                formatter.IndentationLevel--;
            }
            return formatter.ToString();
        }

        public virtual void FormatCustomEventDetails(WebEventFormatter formatter)
        {
        }
    }

    #endregion

    #region WebBaseErrorEventWrapper
    /// <summary>
    /// 
    /// </summary>
    public class WebBaseErrorEventWrapper : WebBaseEventWrapper
    {
        public WebBaseErrorEventWrapper(string message, object eventSource, int eventCode, Exception e)
            : base(message, eventSource, eventCode)
        {
            this.Init(e);
        }
        private void Init(Exception e)
        {
            this._exception = e;
        }
        private Exception _exception;
        public Exception ErrorException
        {
            get
            {
                return this._exception;
            }
        }

        internal override void GenerateFieldsForMarshal(List<WebEventFieldData> fields)
        {
            base.GenerateFieldsForMarshal(fields);

            fields.Add(new WebEventFieldData("ExceptionType", this.ErrorException.GetType().ToString(), WebEventFieldType.String));
            fields.Add(new WebEventFieldData("ExceptionMessage", this.ErrorException.Message, WebEventFieldType.String));

        }

        protected override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
        {
            base.FormatToString(formatter, includeAppInfo);
            if (this._exception != null)
            {
                Exception innerException = this._exception;
                for (int i = 0; (innerException != null) && (i <= 2); i++)
                {
                    formatter.AppendLine(string.Empty);
                    if (i == 0)
                    {
                        formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_exception_information"));
                    }
                    else
                    {
                        formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_inner_exception_information", i.ToString(CultureInfo.InstalledUICulture)));
                    }
                    formatter.IndentationLevel++;
                    formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_exception_type", innerException.GetType().ToString()));
                    formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_exception_message", innerException.Message));
                    formatter.IndentationLevel--;
                    innerException = innerException.InnerException;
                }
            }

        }
    }
    #endregion

    #region WebRequestErrorEventWrapper

    /// <summary>
    /// 
    /// </summary>
    public class WebRequestErrorEventWrapper : WebBaseErrorEventWrapper
    {
        public WebRequestErrorEventWrapper(string message, object eventSource, int eventCode, Exception e)
            : base(message, eventSource, eventCode, e)
        {
        }

        protected override void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
        {
            base.FormatToString(formatter, includeAppInfo);
            formatter.AppendLine(string.Empty);
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_request_information"));
            formatter.IndentationLevel++;
            this.RequestInformation.FormatToString(formatter);
            formatter.IndentationLevel--;
            formatter.AppendLine(string.Empty);
            formatter.AppendLine(WebBaseEventWrapper.FormatResourceStringWithCache("Webevent_event_thread_information"));
            formatter.IndentationLevel++;
            this.ThreadInformation.FormatToString(formatter);
            formatter.IndentationLevel--;

        }
        internal override void GenerateFieldsForMarshal(List<WebEventFieldData> fields)
        {
            base.GenerateFieldsForMarshal(fields);
            fields.Add(new WebEventFieldData("RequestUrl", this.RequestInformation.RequestUrl, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("RequestPath", this.RequestInformation.RequestPath, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("UserHostAddress", this.RequestInformation.UserHostAddress, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("UserName", this.RequestInformation.Principal.Identity.Name, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("UserAuthenticated", this.RequestInformation.Principal.Identity.IsAuthenticated.ToString(), WebEventFieldType.Bool));
            fields.Add(new WebEventFieldData("UserAuthenticationType", this.RequestInformation.Principal.Identity.AuthenticationType, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("RequestThreadAccountName", this.RequestInformation.ThreadAccountName, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("ThreadID", this.ThreadInformation.ThreadID.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Int));
            fields.Add(new WebEventFieldData("ThreadAccountName", this.ThreadInformation.ThreadAccountName, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("StackTrace", this.ThreadInformation.StackTrace, WebEventFieldType.String));
            fields.Add(new WebEventFieldData("IsImpersonating", this.ThreadInformation.IsImpersonating.ToString(), WebEventFieldType.Bool));
        }

        public WebRequestInformation RequestInformation
        {
            get
            {
                this.InitRequestInformation();
                return this._requestInfo;
            }
        }
        private void InitRequestInformation()
        {
            if (this._requestInfo == null)
            {
                this._requestInfo = new WebRequestInformation();
            }
        }
        public WebThreadInformation ThreadInformation
        {
            get
            {
                this.InitThreadInformation();
                return this._threadInfo;
            }
        }
        private void InitThreadInformation()
        {
            if (this._threadInfo == null)
            {
                this._threadInfo = new WebThreadInformation(base.ErrorException);
            }
        }

        private WebThreadInformation _threadInfo;
        private WebRequestInformation _requestInfo;
    }
    #endregion
}