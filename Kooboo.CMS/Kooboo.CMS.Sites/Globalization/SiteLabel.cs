using System;
using System.Linq;
using System.Collections.Generic;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Caching;
using System.Web;
namespace Kooboo.CMS.Sites.Globalization
{
    public static class SiteLabel
    {
        #region Cache
        static string cacheKey = "SiteLabelRepository";
        public static IElementRepository GetElementRepository(Site site)
        {
            var repository = site.ObjectCache().Get(cacheKey);
            if (repository == null)
            {
                repository = new Kooboo.Globalization.Repository.CacheElementRepository(DefaultRepositoryFactory.Instance.CreateRepository(site));
                site.ObjectCache().Add(cacheKey, repository, new System.Runtime.Caching.CacheItemPolicy()
                {
                    SlidingExpiration = TimeSpan.Parse("00:30:00")
                });
            }
            return (IElementRepository)repository;
        }

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

            return Label(defaultValue, key, category, Page_Context.Current.PageRequestContext.Site);
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

            if (Kooboo.Settings.IsWebApplication && Page_Context.Current.EnabledInlineEditing(EditingType.Label))
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
            return RawLabel(defaultValue, key, category, Page_Context.Current.PageRequestContext.Site);
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
            var repository = GetElementRepository(site);


            var element = repository.Get(key, category, "en-US");

            string value = "";
            if (element == null)
            {
                element = new Element() { Name = key, Category = category ?? "", Culture = "en-US", Value = defaultValue };

                repository.Add(element);

                value = element.Value;
            }
            else
            {
                value = element.Value;
            }
            return value;
        }
    }
}
