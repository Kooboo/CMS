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
        public static string Combine(params string[] names)
        {
            return string.Join(NameSplitter, names.Where(it => !string.IsNullOrEmpty(it)).ToArray());
        }
        public static IEnumerable<string> Split(string fullName)
        {
            return fullName.Split(new char[] { '~', '/' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetParentFullName(string fullName)
        {
            var namePaths = Split(fullName).ToArray();

            if (namePaths.Length > 1)
            {
                var parentNamePaths = namePaths.Take(namePaths.Length - 1);

                return Combine(parentNamePaths.ToArray());
            }
            return null;
        }
        public static string GetName(string fullName)
        {
            var namePaths = Split(fullName).ToArray();

            return namePaths[namePaths.Length - 1];
        }

        public static string ToPathName(string fullName)
        {
            return fullName.Replace("~", "\\");
        }
    }
}
