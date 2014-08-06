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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Models
{
    public class DomainSetting
    {
        private string[] domains;
        public string[] Domains
        {
            get { return domains; }
            set
            {
                if (value != null)
                {
                    var regex = new Regex("http(s?)\\://", RegexOptions.IgnoreCase);
                    domains = value.Where(it => !string.IsNullOrEmpty(it)).Select(it => regex.Replace(it, "")).ToArray();
                }
                else
                {
                    domains = value;
                }
            }
        }
        public string SitePath { get; set; }
        public string ResourceDomain { get; set; }
        /// <summary>
        /// 相同同域名根据UserAgent查找不同站点
        /// </summary>
        public string UserAgent { get; set; }
    }
}
