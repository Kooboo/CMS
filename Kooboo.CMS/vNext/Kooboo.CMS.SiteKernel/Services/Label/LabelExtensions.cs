#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.Common.ObjectContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.SiteKernel.Services
{
    public static class LabelExtensions
    {
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

            //if (Kooboo.Settings.IsWebApplication && Page_Context.Current.Initialized && Page_Context.Current.EnabledInlineEditing(EditingType.Label))
            //{
            //    value = string.Format("<var start=\"true\" editType=\"label\" dataType=\"{0}\" key=\"{1}\" category=\"{2}\" style=\"display:none;\"></var>{3}<var end=\"true\" style=\"display:none;\"></var>"
            //        , Kooboo.CMS.Sites.View.FieldDataType.Text.ToString()
            //        , HttpUtility.HtmlEncode(key)
            //        , HttpUtility.HtmlEncode(category)
            //        , value);
            //}

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

            var labelService = EngineContext.Current.Resolve<ILabelService>();

            var label = labelService.GetByName(site, category, key);

            string value = defaultValue;
            if (label == null)
            {
                label = new Label(site, category, key) { Value = defaultValue, UtcCreationDate = DateTime.UtcNow, LastestEditor = editor };
                labelService.Add(label);
            }
            else
            {
                value = label.Value;
            }
            return value;
        }
    }
}
