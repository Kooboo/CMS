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

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class MediaContentUrlExtensions
    {
        /// <summary>
        /// Get the media content url.
        /// </summary>
        /// <param name="fullFoldername"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //public virtual IHtmlString MediaContentUrl(this IFrontUrlHelper frontUrl, string fullFoldername, string fileName)
        //{
        //    var mediaFolder = new Kooboo.CMS.Content.Models.MediaFolder(frontUrl.Site.GetRepository(), fullFoldername);

        //    HtmlString htmlString = new HtmlString("");
        //    if (string.IsNullOrEmpty(fullFoldername))
        //    {
        //        return htmlString;
        //    }
        //    if (string.IsNullOrEmpty(fileName))
        //    {
        //        var folderPath = new FolderPath(mediaFolder);
        //        htmlString = new HtmlString(frontUrl.Url.Content(folderPath.VirtualPath));
        //    }
        //    else
        //    {
        //        var mediaContent = mediaFolder.CreateQuery().WhereEquals("FileName", fileName).FirstOrDefault();
        //        if (mediaContent != null)
        //        {
        //            htmlString = new HtmlString(frontUrl.Url.Content(mediaContent.VirtualPath));
        //        }
        //    }

        //    return htmlString;

        //}

    }
}
