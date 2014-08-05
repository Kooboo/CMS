#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Models
{
    public partial class Site : IIdentifiable, IComparable, IPersistable, ISiteObject
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
        public Site(string fullName)
        {
            this.Name = FullNameHelper.GetName(fullName);
            this.FullName = fullName;
        }
        public Site(Site parent, string name)
        {
            this.Name = name;
            if (parent != null)
            {
                this.FullName = FullNameHelper.Combine(parent.FullName, name);
            }
            else
            {
                this.FullName = name;
            }
        }
        #endregion
        #region Parent
        public Site Parent
        {
            get
            {
                if (string.IsNullOrEmpty(this.FullName))
                {
                    this.FullName = this.Name;
                }
                var parentFullName = FullNameHelper.GetParentFullName(this.FullName);
                if (!string.IsNullOrEmpty(parentFullName))
                {
                    return new Site(parentFullName);
                }
                return null;
            }
            set
            {
                if (Parent != null)
                {
                    this.FullName = FullNameHelper.Combine(Parent.FullName, this.Name);
                }
                else
                {
                    this.FullName = this.Name;
                }
            }

        }
        Site ISiteObject.Site
        {
            get
            {
                return this.Parent;
            }
            set
            {
                this.Parent = value;
            }
        }
        #endregion
        public string UUID
        {
            get
            {
                return this.FullName;
            }
            set
            {
                // null setter
            }
        }

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

        #region IPersistable
        public bool IsDummy
        {
            get { throw new NotImplementedException(); }
        }

        public void Init(IPersistable source)
        {
            throw new NotImplementedException();
        }
        public void OnSaved()
        {
            throw new NotImplementedException();
        }

        public void OnSaving()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
    public partial class Site
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Culture { get; set; }
        public string Theme { get; set; }

        public ReleaseMode Mode { get; set; }

        public DomainSetting DomainSetting { get; set; }

        public bool Published { get; set; }

        public string Version { get; set; }

        public Smtp Smtp { get; set; }

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
        public string TimeZoneId { get; set; }

        /// <summary>
        /// 全局性的HtmlMeta设置
        /// </summary>
        public HtmlMeta HtmlMeta { get; set; }

        public Dictionary<string, string> CustomFields { get; set; }

        // Repository,Membership，还有以后可能的其它模块，是不是反方向设置会比较容易扩展？
        public string Repository { get; set; }
        public string Membership { get; set; }

        /// <summary>
        /// 可能不需要，用插件形式扩展引用JQuery
        /// </summary>
        public bool EnableJQuery { get; set; }
        /// <summary>
        /// 可能不需要，做成插件扩展会更好
        /// </summary>
        public bool EnableInlineEditing { get; set; }
        public bool EnableVersioning { get; set; }
        public bool EnableStyleEditing { get; set; }

        ///用扩展实现
        //KeyValue<string, string> SSLDetection { get; set; }


    }
}
