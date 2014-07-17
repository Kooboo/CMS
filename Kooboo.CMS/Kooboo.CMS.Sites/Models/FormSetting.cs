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

namespace Kooboo.CMS.Sites.Models
{
    public enum SubmitType
    {
        Normal,
        AJAX
    }
    public class FormSetting
    {
        public string Name { get; set; }
        public string PluginType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SubmitType SubmitType { get; set; }
        public string RedirectTo { get; set; }

        public Dictionary<string, string> Settings { get; set; }
    }
}
