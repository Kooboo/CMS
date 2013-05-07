#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
#region Usings

using System;
using System.IO;

#endregion

[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.Caching.AzureSync.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.Caching.AzureSync
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyInitializer
    {
        #region Methods
        public static void Initialize()
        {
            ApplicationInitialization.RegisterInitializerMethod(delegate()
            {
                Kooboo.CMS.Caching.CacheExpiredNotification.Notifiactions.Add(new AzureInstancesCachingNotification());
            }, 0);
        }

        #endregion
    }
}
