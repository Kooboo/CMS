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
using System.Web;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class ResizeImageLinkExtensions
    {
        #region ResizeImage
        public static IHtmlString ResizeImage(this IFrontHtmlHelper frontHtml, string imagePath, int width, int height)
        {
            return ResizeImage(frontHtml, imagePath, width, height, null, null, null);
        }
        public static IHtmlString ResizeImage(this IFrontHtmlHelper frontHtml, string imagePath, int width, int height, string alt)
        {
            return ResizeImage(frontHtml, imagePath, width, height, null, null, alt);
        }
        public static IHtmlString ResizeImage(this IFrontHtmlHelper frontHtml, string imagePath, int width, int height, bool? preserverAspectRatio, int? quality, string alt)
        {
            return new HtmlString(string.Format("<img src=\"{0}\" alt=\"{1}\" />"
                , frontHtml.Page_Context.FrontUrl.ResizeImageUrl(imagePath, width, height, preserverAspectRatio, quality)
                , alt));
        }
        #endregion
    }
}
