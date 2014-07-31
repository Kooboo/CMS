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

using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.SiteKernel.Models
{
    public static class SiteExtensions
    {
        public static string PREFIX_FRONT_DEBUG_URL = "dev~";

        public static string GetVersionUsedInUrl(this Site site)
        {
            StringBuilder sb = new StringBuilder();
            while (site != null)
            {
                sb.AppendFormat("{0}!", (site.AsActual().Version ?? "1.0.0.0").Replace(".", "_"));
                site = site.Parent;
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        public static Repository GetRepository(this Site site)
        {
            site = site.AsActual();
            if (site != null && !string.IsNullOrEmpty(site.Repository))
            {
                return new Repository(site.Repository).AsActual();
            }
            return null;
        }
    }
}
