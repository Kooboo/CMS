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
using Kooboo.Common.Globalization;
using System.Collections;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Versioning
{
    public class VersionManager
    {
        static Hashtable loggers = new Hashtable();
        static VersionManager()
        {
            VersionManager.RegisterVersionLogger<Page>(new Kooboo.CMS.Sites.Persistence.FileSystem.PageProvider.PageVersionLogger());
            VersionManager.RegisterVersionLogger<Layout>(new Kooboo.CMS.Sites.Persistence.FileSystem.LayoutProvider.LayoutVersionLogger());
            VersionManager.RegisterVersionLogger<Kooboo.CMS.Sites.Models.View>(new Kooboo.CMS.Sites.Persistence.FileSystem.ViewProvider.ViewVersionLogger());
            VersionManager.RegisterVersionLogger<HtmlBlock>(new Kooboo.CMS.Sites.Persistence.FileSystem.HtmlBlockProvider.HtmlBlockVersionLogger());
        }
        public static void RegisterVersionLogger<T>(IVersionLogger<T> versionLogger)
            where T : DirectoryResource
        {
            lock (loggers)
            {
                loggers[typeof(T)] = versionLogger;
            }
        }
        public static IVersionLogger<T> ResolveVersionLogger<T>()
                          where T : DirectoryResource
        {
            IVersionLogger<T> logger = null;
            var type = typeof(T);
            if (loggers.ContainsKey(type))
            {
                logger = (IVersionLogger<T>)loggers[type];
            }
            if (logger == null)
            {
                throw new Exception(string.Format("There has not version logger for '{0}'".Localize(), type));
            }
            return logger;
        }
        public static void LogVersion<T>(T o)
                        where T : DirectoryResource
        {
            if (Site.Current == null || (Site.Current.EnableVersioning.HasValue ? Site.Current.EnableVersioning.Value : true))
            {
                ResolveVersionLogger<T>().LogVersion(o);
            }
        }
        public static IEnumerable<VersionInfo> AllVersions<T>(T o)
                        where T : DirectoryResource
        {
            return ResolveVersionLogger<T>().AllVersions(o);
        }

        public static T GetVersion<T>(T o, int version)
                        where T : DirectoryResource
        {
            return ResolveVersionLogger<T>().GetVersion(o, version);
        }
        public static void Revert<T>(T o, int version, string userName)
              where T : DirectoryResource
        {
            ResolveVersionLogger<T>().Revert(o, version, userName);
        }
    }
}
