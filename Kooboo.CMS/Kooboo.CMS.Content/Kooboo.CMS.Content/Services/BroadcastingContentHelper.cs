using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using System.Collections.Specialized;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Content.Services
{
    /// <summary>
    /// 内容广播的辅助方法
    /// </summary>
    public static class BroadcastingContentHelper
    {
        private static List<string> contentBasicFields = new List<string>()
            {"Id",
            "UUID",
            "Repository",
            "FolderName",
            "UserKey",
            "UtcCreationDate",
            "UtcLastModificationDate",
            "Published",
            "OriginalUUID",
            "OriginalRepository",
            "OriginalFolder",
            "IsLocalized"
            };
        /// <summary>
        /// 排序系统字段的其它自定义字段值
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static NameValueCollection ExcludeBasicFields(ContentBase content)
        {
            NameValueCollection values = new NameValueCollection();
            foreach (var key in content.Keys.Where(key => !contentBasicFields.Contains(key, StringComparer.CurrentCultureIgnoreCase)))
            {
                if (content[key] != null)
                {
                    values[key] = content[key].ToString();
                }
            }
            return values;
        }
        /// <summary>
        /// 根据源内容查询广播的内容
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="originalUUID">The original UUID.</param>
        /// <returns></returns>
        public static IEnumerable<TextContent> GetContentsByOriginalUUID(TextFolder folder, string originalUUID)
        {
            return folder.CreateQuery().WhereEquals("OriginalUUID", originalUUID);
        }
    }
}
