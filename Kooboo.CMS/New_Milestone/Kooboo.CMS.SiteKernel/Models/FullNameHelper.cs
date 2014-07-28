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
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Models
{
    public static class FullNameHelper
    {
        public static string NameSplitter = "~";
        public static string Combine(IEnumerable<string> names)
        {
            return string.Join(NameSplitter, names.Where(it => !string.IsNullOrEmpty(it)).ToArray());
        }
        public static IEnumerable<string> Split(string fullName)
        {
            return fullName.Split(new char[] { '~', '/' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
