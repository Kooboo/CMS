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

namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class FolderHelper
    {
        /// <summary>
        /// 合成完整名称
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        public static string CombineFullName(IEnumerable<string> names)
        {
            return string.Join("~", names.ToArray());
        }
        /// <summary>
        /// 分隔完整名称
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitFullName(string fullName)
        {
            return fullName.Split(new char[] { '~', '/' }, StringSplitOptions.RemoveEmptyEntries);
        }
        /// <summary>
        /// 根据完整名称取得目录对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>        
        public static T Parse<T>(Repository repository, string fullName)
            where T : Folder
        {
            var names = SplitFullName(fullName);
            if (typeof(T) == typeof(CMS.Content.Models.TextFolder))
            {
                if (names == null || !names.Any())
                {
                    var defaultFolder = new List<string>();
                    defaultFolder.Add(Paths.FolderPath.GetBaseDir<TextFolder>(repository));
                    names = names.Concat(defaultFolder);
                }
                return (T)((object)new TextFolder(repository, names));
            }
            else
            {
                if (names == null || !names.Any())
                {
                    var defaultFolder = new List<string>();
                    defaultFolder.Add(Paths.FolderPath.GetBaseDir<MediaFolder>(repository));
                    names = names.Concat(defaultFolder);
                }
                return (T)((object)new MediaFolder(repository, names));
            }
        }
    }
}
