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
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using Kooboo.Globalization;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Url;
namespace Kooboo.CMS.Sites.View
{
    /// <summary>
    /// 封装View中做查询需要创建Folder的代码
    /// </summary>
    public class ContentHelper
    {
        public static MediaFolder MediaFolder(string folderName)
        {
            return new MediaFolder(Repository.Current, folderName);
        }
        [Obsolete("MediaFolder")]
        public static MediaFolder NewMediaFolderObject(string folderName)
        {
            return MediaFolder(folderName);
        }

        public static TextFolder TextFolder(string folderName)
        {
            return new TextFolder(Repository.Current, folderName);
        }

        [Obsolete("TextFolder")]
        public static TextFolder NewTextFolderObject(string folderName)
        {
            return TextFolder(folderName);
        }

        public static Schema Schema(string schemaName)
        {
            return new Schema(Repository.Current, schemaName);
        }
        [Obsolete("Schema")]
        public static Schema NewSchemaObject(string schemaName)
        {
            return Schema(schemaName);
        }
        public static IEnumerable<string> SplitMultiFiles(string files)
        {
            if (string.IsNullOrEmpty(files))
            {
                return new string[0];
            }
            return files.Split('|').Select(it => Kooboo.Web.Url.UrlUtility.ResolveUrl(it)).ToArray();
        }

        public static MediaContent ParseMediaContent(string virtualPath)
        {
            return ParseMediaContent(Site.Current.GetRepository(), virtualPath);
        }
        public static MediaContent ParseMediaContent(Repository repository, string virtualPath)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute))
            {
                throw new NotSupportedException("Not support to parse absolute url path.".Localize());
            }
            var mediaBaseVirtualPath = UrlUtility.Combine(new RepositoryPath(repository).VirtualPath, FolderPath.GetRootPath(typeof(MediaFolder)));
            var mediaFolderVirtualPath = UrlUtility.ResolveUrl(mediaBaseVirtualPath);
            if (virtualPath.Length > mediaFolderVirtualPath.Length)
            {
                var mediaContentPath = virtualPath.Substring(mediaBaseVirtualPath.Length - 1);
                var paths = mediaContentPath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                var mediaFolder = new MediaFolder(repository, paths.Take(paths.Length - 1));
                var fileName = paths.Last();
                return mediaFolder.CreateQuery().WhereEquals("UUID", fileName).FirstOrDefault();
            }
            return null;
        }
    }
}
