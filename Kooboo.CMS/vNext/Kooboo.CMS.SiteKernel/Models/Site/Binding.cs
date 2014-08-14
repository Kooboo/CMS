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
    public class Binding
    {
        static Regex regex = new Regex("http(s?)\\://", RegexOptions.IgnoreCase);

        private string domain;
        public string Domain
        {
            get { return domain; }
            set
            {
                if (value != null)
                {

                    domain = regex.Replace(value, "");
                }
                else
                {
                    domain = value;
                }
            }
        }
        public string SitePath { get; set; }        
        /// <summary>
        /// 相同同域名根据UserAgent查找不同站点
        /// Device == UserAgent
        /// </summary>
        public string Device { get; set; }
    }
}
