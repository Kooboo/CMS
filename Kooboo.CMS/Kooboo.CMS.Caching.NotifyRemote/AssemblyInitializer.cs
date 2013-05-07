#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.Caching.NotifyRemote.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.Caching.NotifyRemote
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
                Kooboo.CMS.Caching.CacheExpiredNotification.Notifiactions.Add(new NotifyRemoteServer());
            }, 0);
        } 
        #endregion
    }
}
