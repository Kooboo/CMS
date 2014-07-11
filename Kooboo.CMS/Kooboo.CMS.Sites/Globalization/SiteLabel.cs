#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Linq;
using System.Collections.Generic;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.Common.Globalization;
using Kooboo.CMS.Sites.Caching;
using System.Web;
using Kooboo.CMS.Sites.Services;
using Kooboo.Common;
namespace Kooboo.CMS.Sites.Globalization
{
    public static class SiteLabel
    {
        #region Cache
        static string cacheKey = "SiteLabelRepository";
        [Obsolete]
        public static IElementRepository GetElementRepository(Site site)
        {
            var repository = site.ObjectCache().Get(cacheKey);
            if (repository == null)
            {
                repository = new Kooboo.Common.Globalization.Repository.CacheElementRepository(() => Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<IElementRepositoryFactory>().CreateRepository(site));
                site.ObjectCache().Add(cacheKey, repository, new System.Runtime.Caching.CacheItemPolicy()
                {
                    SlidingExpiration = TimeSpan.Parse("00:30:00")
                });
            }
            return (IElementRepository)repository;
        }
        [Obsolete]
        public static void ClearCache(Site site)
        {
            site.ObjectCache().Remove(cacheKey);
        }
        #endregion

        #region Label with inline-editing.
        /// <summary>
        /// Label with inline-editing.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static IHtmlString Label(this string defaultValue)
        {
            return Label(defaultValue, defaultValue);
        }
        /// <summary>
        /// Label with inline-editing.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="key">The key.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public static IHtmlString Label(this string defaultValue, string key, string category = "")
        {
            //var pageViewContext = Page_Context.Current;

            //pageViewContext.CheckContext();

            return Label(defaultValue, key, category, Site.Current);
        }
        /// <summary>
        /// Label with inline-editing.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="key">The key.</param>
        /// <param name="category">The category.</param>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        public static IHtmlString Label(this string defaultValue, string key, string category, Site site)
        {
            string value = LabelValue(defaultValue, key, category, site);

            if (Settings.IsWebApplication && Page_Context.Current.Initialized && Page_Context.Current.EnabledInlineEditing(EditingType.Label))
            {
                value = string.Format("<var start=\"true\" editType=\"label\" dataType=\"{0}\" key=\"{1}\" category=\"{2}\" style=\"display:none;\"></var>{3}<var end=\"true\" style=\"display:none;\"></var>"
                    , Kooboo.CMS.Sites.View.FieldDataType.Text.ToString()
                    , HttpUtility.HtmlEncode(key)
                    , HttpUtility.HtmlEncode(category)
                    , value);
            }

            return new HtmlString(value);
        }
        #endregion

        #region RawLabel Label without inline editing.
        /// <summary>
        /// Raws the label. Label without inline editing.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static IHtmlString RawLabel(this string defaultValue)
        {
            return RawLabel(defaultValue, defaultValue);
        }
        /// <summary>
        /// Raws the label. Label without inline editing.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="key">The key.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public static IHtmlString RawLabel(this string defaultValue, string key, string category = "")
        {
            return RawLabel(defaultValue, key, category, Site.Current);
        }
        /// <summary>
        /// Raws the label. Label without inline editing.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="key">The key.</param>
        /// <param name="category">The category.</param>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        public static IHtmlString RawLabel(this string defaultValue, string key, string category, Site site)
        {
            return new HtmlString(LabelValue(defaultValue, key, category, site));
        }

        #endregion

        private static string LabelValue(string defaultValue, string key, string category, Site site)
        {
            if (string.IsNullOrEmpty(key) && string.IsNullOrEmpty(category))
            {
                return defaultValue;
            }

            string editor = null;

            if (HttpContext.Current != null)
            {
                editor = HttpContext.Current.User.Identity.Name;
            }

            var labelManager = ServiceFactory.LabelManager;

            var label = labelManager.Get(site, category, key);

            string value = defaultValue;
            if (label == null)
            {
                label = new Label(site, category, key, defaultValue) { UtcCreationDate = DateTime.UtcNow, LastestEditor = editor };
                labelManager.Add(site, label);
            }
            else
            {
                value = label.Value;
            }
            return value;
        }
    }
}
