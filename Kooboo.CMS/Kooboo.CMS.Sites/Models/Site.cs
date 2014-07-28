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
using Kooboo.CMS.Sites.Models;
using System.Runtime.Serialization;
using System.IO;


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Kooboo.CMS.Common.Persistence.Non_Relational;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Web;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Common;
using Kooboo.Common.Misc;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Sites.Models
{
    #region ReleaseMode
    public enum ReleaseMode
    {
        Debug = 0,
        Release = 1
    }
    #endregion

    #region SiteHelper
    public static class SiteHelper
    {
        public static string PREFIX_FRONT_DEBUG_URL = "dev~";

        public static char DELIMITER = '~';

        public static string CombineFullName(IEnumerable<string> pageNames)
        {
            return string.Join(DELIMITER.ToString(), pageNames.ToArray());
        }
        public static IEnumerable<string> SplitFullName(string fullName)
        {
            return fullName.Split(DELIMITER, '/');
        }
        public static Site Parse(string fullName)
        {
            return new Site(SplitFullName(fullName));
        }
    }
    #endregion

    #region Smtp
    public class Smtp
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        private int port = 25;
        public int Port { get { return port; } set { port = value; } }
        public bool EnableSsl { get; set; }
        public string[] To { get; set; }
        public string From { get; set; }

        public SmtpClient ToSmtpClient()
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = this.Host;
            smtpClient.Port = this.Port;
            smtpClient.EnableSsl = this.EnableSsl;
            if (!string.IsNullOrEmpty(this.UserName))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(this.UserName, this.Password);
            }
            return smtpClient;
        }

    }
    #endregion

    #region Site
    public partial class Site : DirectoryResource, ISiteObject, IFilePersistable, IPersistable, IIdentifiable, IComparable
    {
        #region Current
        public static Site Current
        {
            get
            {
                return ContextVariables.Current.GetObject<Site>("Current_Site");
            }
            set
            {
                ContextVariables.Current.SetObject("Current_Site", value);
            }
        }
        #endregion

        #region .ctor
        public Site()
        {

        }
        public Site(string name)
            : this(SiteHelper.SplitFullName(name))
        {

        }
        public Site(Site parent, string name)
            : this(name)
        {
            this.Parent = parent;
        }

        public Site(IEnumerable<string> namePath)
        {
            if (namePath == null || namePath.Count() < 1)
            {
                throw new ArgumentException("The folder name path is invalid.", "namePath");
            }
            this.Name = namePath.Last();
            if (namePath.Count() > 0)
            {
                foreach (var name in namePath.Take(namePath.Count() - 1))
                {
                    this.Parent = new Site(this.Parent, name);
                }
            }

        }
        #endregion

        #region Computed Properties
        string _fullName = null;
        public string FullName
        {
            get
            {

                _fullName = SiteHelper.CombineFullName(RelativePaths.ToArray());

                return _fullName;
            }
        }
        public string FriendlyName
        {
            get
            {
                var display = this.AsActual().DisplayName;
                if (String.IsNullOrEmpty(display))
                {
                    display = this.Name;
                }

                if (parent == null)
                {
                    return display;
                }
                else
                {
                    return parent.AsActual().FriendlyName + " / " + display;
                }
            }
        }
        /// <summary>
        /// 站点的所有完整域，所有域名加上SitePath
        /// </summary>
        public IEnumerable<string> FullDomains
        {
            get
            {
                var domains = this.Domains ?? new string[0];
                var sitePath = this.SitePath == null ? "" : this.SitePath.Trim().Trim('/');
                var portSegment = "";
                if (HttpContext.Current != null)
                {
                    if (!(HttpContext.Current.Request.Url.Port == 80 || HttpContext.Current.Request.Url.Port == 443))
                    {
                        portSegment = ":" + HttpContext.Current.Request.Url.Port;
                    }
                }
                return domains.Where(it => !string.IsNullOrEmpty(it)).Select(it => (it.Trim('/') + portSegment + "/" + sitePath).TrimEnd('/') + "/");
            }
        }
        #endregion

        #region PysicalPath VirutalPath
        /// <summary>
        /// The physical base path of site
        /// </summary>
        /// <value>The physical path.</value>
        public override string PhysicalPath
        {
            get
            {
                return Path.Combine(BasePhysicalPath, this.Name);
            }
        }

        public override string VirtualPath
        {
            get
            {
                return Kooboo.Common.Web.UrlUtility.Combine(BaseVirtualPath, this.Name);
            }
        }

        public string ChildSitesBasePhysicalPath
        {
            get
            {
                return Path.Combine(this.PhysicalPath, PATH_NAME);
            }
        }
        public string ChildSitesBaseVirualPath
        {
            get
            {
                return UrlUtility.Combine(this.VirtualPath, PATH_NAME);
            }
        }


        #region static

        public static string RootBasePhysicalPath
        {
            get
            {
                var baseDir = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<IBaseDir>();
                return Path.Combine(baseDir.Cms_DataPhysicalPath, PATH_NAME);
            }
        }
        public static readonly string PATH_NAME = "Sites";

        public override string BasePhysicalPath
        {
            get
            {
                if (Parent == null)
                {
                    return RootBasePhysicalPath;
                }
                else
                {
                    return Path.Combine(Parent.PhysicalPath, PATH_NAME);
                }
            }
        }
        public override string BaseVirtualPath
        {
            get
            {
                if (Parent == null)
                {
                    return Kooboo.Common.Web.UrlUtility.Combine("~/", PathEx.BasePath, PATH_NAME);
                }
                else
                {
                    return UrlUtility.Combine(Parent.VirtualPath, PATH_NAME);
                }

            }
        }

        //private IEnumerable<string> relativePaths;
        public override IEnumerable<string> RelativePaths
        {
            get
            {
                var current = new string[] { this.Name };
                if (Parent == null)
                {
                    //如果有启用继承，需要一级一级组建relativePath
                    return current;
                }
                else
                {
                    return parent.RelativePaths.Concat(current);
                }
            }

        }

        /// <summary>
        /// Trims the base physical path.
        /// </summary>
        /// <param name="physicalPath">The physical path.<example>d:\cms\sites\site1\themes\default</example></param>
        /// <returns>trimed physical path <example>themes\default</example> </returns>
        public static string TrimBasePhysicalPath(string physicalPath)
        {
            return physicalPath.Replace(Site.RootBasePhysicalPath, "").Trim(Path.DirectorySeparatorChar);
        }
        /// <summary>
        /// Parses the site from physical path.
        /// </summary>
        /// <param name="physicalPath">The physical path.</param>
        /// <returns></returns>
        public static Site ParseSiteFromPhysicalPath(string physicalPath)
        {
            var trimedPath = TrimBasePhysicalPath(physicalPath);
            return ParseSiteFromRelativePath(trimedPath.Split(Path.DirectorySeparatorChar));
        }
        /// <summary>
        /// Parses the site from relative path.
        /// </summary>
        /// <param name="relativePaths">The relative paths.</param>
        /// <returns></returns>
        public static Site ParseSiteFromRelativePath(IEnumerable<string> relativePaths)
        {
            //有继承的情况将会是： site1\sites\site2\sites\site3
            //或是 site1\site2\site3

            Site site = null;
            foreach (var siteName in relativePaths.Where(it => !it.Equals(PATH_NAME, StringComparison.InvariantCultureIgnoreCase)))
            {
                site = new Site(site, siteName);
            }
            return site;
        }

        #endregion

        #endregion

        #region Parent
        private Site parent;
        public Site Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
                _fullName = null;
            }
        }

        #endregion

        #region override object
        public static bool operator ==(Site obj1, Site obj2)
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
        public static bool operator !=(Site obj1, Site obj2)
        {
            return !(obj1 == obj2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Site))
            {
                return false;
            }
            var site = (Site)obj;
            if (this.FullName.EqualsOrNullEmpty(site.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return this.FullName;
        }
        #endregion

        #region IPersistable Members
        public string UUID
        {
            get
            {
                return this.FullName;
            }
            set
            {
                this._fullName = value;
            }
        }
        #region IDummy Members
        private bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            private set { isDummy = value; }
        }

        #endregion

        void IPersistable.Init(IPersistable source)
        {
            this.IsDummy = false;

            var site = (Site)source;
            this.Name = site.Name;
            this.Parent = site.Parent;
        }
        void IPersistable.OnSaved()
        {
            this.IsDummy = false;
        }
        public string DataFile
        {
            get { return Path.Combine(this.PhysicalPath, SettingFileName); }
        }

        void IPersistable.OnSaving()
        {

        }

        #endregion

        #region override base
        public override bool Exists()
        {
            return File.Exists(this.DataFile);
        }

        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            //this.FileName = relativePaths.Last();

            return relativePaths.Take(relativePaths.Count() - 1);
        }
        #endregion

        #region IComparable
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            return this.FullName.CompareTo(((Site)obj).FullName);
        }
        #endregion
        #region ISiteObject
        Site ISiteObject.Site
        {
            get { return this; }
            set { }
        }
        #endregion
    }
    #endregion

    #region Security
    public class Security
    {
        public Security()
        {
            this.EncryptKey = UniqueIdGenerator.GetInstance().GetBase32UniqueId(8);
        }
        public bool TurnOnSubmissionAPI { get; set; }
        public string EncryptKey { get; set; }
    }
    #endregion

    #region Persistence data
    [DataContract]
    public partial class Site
    {
        [DataMember(Order = 2)]//
        public string DisplayName { get; set; }

        [DataMember(Order = 4)]
        public string Culture
        {
            get;
            set;
        }

        [DataMember(Order = 6)]//
        public string Theme
        {
            get;
            set;
        }

        private string[] domains;
        [DataMember(Order = 8)]//
        public string[] Domains
        {
            get { return domains; }
            set
            {
                if (value != null)
                {
                    var regex = new Regex("http(s?)\\://", RegexOptions.IgnoreCase);
                    domains = value.Select(it => regex.Replace(it, "")).ToArray();
                }
                else
                {
                    domains = value;
                }
            }
        }

        private string _sitePath;
        [DataMember(Order = 9)]
        public string SitePath
        {
            get
            {
                return _sitePath;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _sitePath = value.Trim('/');
                }
                else
                {
                    _sitePath = value;
                }
            }
        }


        [DataMember(Order = 10)]//
        public ReleaseMode Mode { get; set; }

        private string version = "1.0.0.0";
        [DataMember(Order = 12)]//
        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }

        public string VersionUsedInUrl
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                var site = this;
                while (site != null)
                {
                    sb.AppendFormat("{0}!", (site.AsActual().Version ?? "1.0.0.0").Replace(".", "_"));
                    site = site.Parent;
                }
                return sb.Remove(sb.Length - 1, 1).ToString();
            }
        }

        [DataMember(Order = 13)]
        public string Repository { get; set; }

        [DataMember(Order = 15)]
        public bool EnableJquery { get; set; }

        private bool? inlineEditing = true;
        [DataMember(Order = 16)]
        public bool? InlineEditing
        {
            get
            {
                if (!inlineEditing.HasValue)
                {
                    inlineEditing = true;
                }
                return inlineEditing;
            }
            set
            {
                inlineEditing = value;
            }
        }

        [DataMember(Order = 18)]
        public Smtp Smtp { get; set; }

        private bool? showSitemap = true;
        [DataMember(Order = 20)]
        public bool? ShowSitemap
        {
            get
            {
                if (!showSitemap.HasValue)
                {
                    showSitemap = true;
                }
                return showSitemap;
            }
            set
            {
                showSitemap = value;
            }
        }

        private Dictionary<string, string> customFields;
        [DataMember(Order = 21)]
        public Dictionary<string, string> CustomFields
        {
            get
            {
                return customFields ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
            set
            {
                if (value != null)
                {
                    customFields = new Dictionary<string, string>(value, StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    customFields = value;
                }
            }
        }

        private bool? enableVersioning = true;
        [DataMember(Order = 22)]
        public bool? EnableVersioning
        {
            get
            {
                if (enableVersioning == null)
                {
                    enableVersioning = true;
                }
                return enableVersioning;
            }
            set
            {
                enableVersioning = value;
            }
        }

        private bool? enableStyleEdting = false;
        [DataMember(Order = 23)]
        public bool? EnableStyleEdting
        {
            get
            {
                if (enableStyleEdting == null)
                {
                    enableStyleEdting = false;
                }
                return enableStyleEdting;
            }
            set
            {
                enableStyleEdting = value;
            }
        }

        [DataMember(Order = 24)]
        public string ResourceDomain { get; set; }


        /// <summary>
        /// 1  站点的设置加了一个时区设置
        /// 2. 在添加内容的时候，所有的时间都认为是这个时区的时间
        /// 3. 在存储到数据库的时候，自动把时间转换成UTC世界
        /// 4. 在查询和独取得时候，会自动把UTC 时间转换成站点设置的时区时间 
        /// 5. 开发人员的代码中，需要保存的时间和使用的时间，都是假设站点时区的时间
        /// 6. 遇到像生日之类的，与时区无关的时间，开发人员可以设计用datetimepicker控件，但是存储的时候使用string存储。避免具有时间的时区性。
        /// </summary>
        /// <value>
        /// The time zone id.
        /// </value>
        [DataMember(Order = 25)]
        public string TimeZoneId { get; set; }

        private Security _security = new Security();
        [DataMember(Order = 26)]
        public Security Security
        {
            get
            {
                if (_security == null)
                {
                    _security = new Security();
                }
                return _security;
            }
            set
            {
                _security = value;
            }
        }
        [DataMember()]//
        public HtmlMeta HtmlMeta { get; set; }

        [DataMember]
        public string Membership { get; set; }

        /// <summary>
        /// Gets or sets the SSL detection.
        /// To detect the request if is a SSL request accoding to the HTTP header/Querystring.
        /// It is because in the master/slave mode, the slave server will only get HTTP request even the user sending a HTTPS request. The master server will send the HTTPS flag via HTTP header or querystring. 
        /// see:https://github.com/plack/Plack/wiki/How-to-detect-reverse-proxy-and-SSL-frontend
        /// </summary>
        /// <value>
        /// The SSL detection.
        /// </value>
        [DataMember]
        public KeyValue<string, string> SSLDetection { get; set; }

        /// <summary>
        /// Gets or sets the user agent.
        /// Matching the device when resolving the site.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        [DataMember]
        public string UserAgent { get; set; }
    }
    #endregion
}
