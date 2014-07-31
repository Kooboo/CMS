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
    public static class ResizeImageUrlExtensions
    {
        /// <summary>
        /// Resizes the image URL.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static IHtmlString ResizeImageUrl(this IFrontUrlHelper frontUrl, string imagePath, int width, int height)
        {
            return ResizeImageUrl(frontUrl, imagePath, width, height, null, null);
        }
        /// <summary>
        /// Resizes the image URL.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="preserverAspectRatio">The preserver aspect ratio.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static IHtmlString ResizeImageUrl(this IFrontUrlHelper frontUrl, string imagePath, int width, int height, bool? preserverAspectRatio, int? quality)
        {
            return frontUrl.ResourceCDNUrl(frontUrl.WrapperUrl(frontUrl.Url.Action("ResizeImage", "Resource",
                new { siteName = frontUrl.Site.FullName, url = imagePath, area = "", width = width, height = height, preserverAspectRatio = preserverAspectRatio, quality = quality })).ToString());
        }

    }
}
