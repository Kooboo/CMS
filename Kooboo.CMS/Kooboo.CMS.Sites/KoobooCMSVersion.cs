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
using System.Reflection;
namespace Kooboo.CMS.Sites
{
    public static class KoobooCMSVersion
    {
        static KoobooCMSVersion()
        {
            Assembly sitesAssembly = typeof(KoobooCMSVersion).Assembly;
            Version = sitesAssembly.GetName().Version.ToString();
        }
        public static string Version { get; private set; }
    }
}
