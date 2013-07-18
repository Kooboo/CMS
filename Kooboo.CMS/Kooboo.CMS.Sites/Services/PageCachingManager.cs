#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Sites.Services
{
    public class PageCachingManager
    {
        public virtual void AddCaching(HttpContextBase httpContext, Page page, string html)
        {
            var filePath = GetFilePath(page, httpContext.Request.Path);
            if (!File.Exists(filePath))
            {
                IOUtility.SaveStringToFile(filePath, html);
            }
        }
        public virtual string GetCaching(HttpContextBase httpContext, Page page)
        {
            string html = null;
            var filePath = GetFilePath(page, httpContext.Request.Path);
            if (File.Exists(filePath))
            {
                var expired = false;
                if (page.OutputCache != null && page.OutputCache.Duration != 0)
                {
                    DateTime cacheTime = DateTime.Now;
                    switch (page.OutputCache.ExpirationPolicy)
                    {
                        case ExpirationPolicy.AbsoluteExpiration:
                            cacheTime = File.GetCreationTime(filePath);
                            break;
                        case ExpirationPolicy.SlidingExpiration:
                            cacheTime = File.GetLastAccessTime(filePath);
                            break;
                    }

                    expired = (DateTime.Now - cacheTime).TotalSeconds > page.OutputCache.Duration;
                }

                try
                {
                    if (File.Exists(filePath))
                    {
                        if (expired == true)
                        {

                            File.Delete(filePath);
                        }
                        else
                        {
                            html = IOUtility.ReadAsString(filePath);
                        }
                    }
                }
                catch (Exception e)
                {
                    Kooboo.HealthMonitoring.Log.LogException(e);
                }

            }
            return html;
        }
        public virtual void DeleteCaching(Page page)
        {
            var pageCachingPage = GetPageCachingPath(page);

            IOUtility.DeleteDirectory(pageCachingPage, true);
        }
        protected virtual string GetFilePath(Page page, string requestPath)
        {
            requestPath = requestPath.TrimEnd('/');
            var extension = Path.GetExtension(requestPath);
            if (string.IsNullOrEmpty(extension))
            {
                requestPath = requestPath + "/default.html";
            }
            return Path.Combine(GetPageCachingPath(page), requestPath.TrimStart('/'));
        }

        protected virtual string GetPageCachingPath(Page page)
        {
            return Path.Combine(page.Site.PhysicalPath, "PageCaching", page.FullName);
        }

    }
}
